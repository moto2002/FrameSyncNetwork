using UnityEngine;
using Google.ProtocolBuffers;
using System.Collections.Generic;
using Utility;
using System;
using messages;
public  class UdpNetManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private int nId = 0;
    private int mId = 0;
    private static UdpNetManager m_instance;
    private List<UdpNetBehaviour> behs = new List<UdpNetBehaviour>();
    LenthFixedQueue<GenMessage> latestCommands = new LenthFixedQueue<GenMessage>(FrameController.PushBack * 4);

    public int FutureFrame
    {
        get;
        private set;
    }
    public int NextNetId
    {
        get {
            return nId++;
        }
    }

    public int NextMsgId
    {
        get
        {
            return mId++;
        }
    }

    public static UdpNetManager Instance
    {
        get {
            if (m_instance == null)
            {
                GameObject go = new GameObject();
                go.AddComponent<UdpNetManager>();
            }
            return m_instance;
        }
    }

    public void AddBehaviour(UdpNetBehaviour item)
    {
        behs.Add(item);
    }

    void Awake()
    {
        m_instance = this;
    }

    void Start()
    {
		foreach (var item in UserInfo.Instance.Room.players) {
			FrameController.Instance.AddPlayer(item, FrameController.ExecuteFrame);
		}
        FrameController.Instance.SortPlayers();
        //FrameController.Instance.Freezed = true;
        MessageDispatcher.Instance.RegisterMsgType(MessageType.Rpc, OnRpcMsgCallback);
        MessageDispatcher.Instance.RegisterMsgType(MessageType.CreateObj, OnCreateObjCallback);
        MessageDispatcher.Instance.RegisterMsgType(MessageType.ReadyForGame, OnReadyForGameCallBack);
        MessageDispatcher.Instance.RegisterMsgType(MessageType.Empty, OnEmptyCallBack);
        MessageDispatcher.Instance.Connect();
        ReadyForGame();
    }

    void OnDestroy()
    {
        m_instance = null;
    }
    void OnEmptyCallBack(int frame, string pId, ByteString bb)
    {
        FrameController.Instance.GetPlayer(pId).GetCommand(new EmptyObj(frame));
    }
    void OnRpcMsgCallback(int frame, string pId, ByteString bb)
    {
        var msg = MsgRpc.ParseFrom(bb);
        FrameController.Instance.GetPlayer(pId).GetCommand(new RpcExeObj(frame, msg));
    }

    void OnCreateObjCallback(int frame, string pId, ByteString bb)
    {
        var msg = MsgCreateObj.ParseFrom(bb);
        FrameController.Instance.GetPlayer(pId).GetCommand(new CreateObjCmd(frame, pId, msg));
    }

    void OnReadyForGameCallBack(int frame, string pId, ByteString bb)
    {
        FrameController.Instance.GetPlayer(pId).GetCommand(CreateObjCmd.CreatePlayer(frame, pId, playerPrefab));
    }

    void Request(MessageType type, int frame, IMessage msg)
    {
        if (msg != null)
        {
            Request(type, frame, msg.ToByteString());
        }
        else {
            Request(type, frame, ByteString.CopyFrom(new byte[]{}));
        }
    }

    void Request(MessageType type, int frame, ByteString buf)
    {
        var genMessage = GenMessage.CreateBuilder()
            .SetMsgType(type)
            .SetFrame(frame)
            .SetPId(UserInfo.Instance.Id)
            .SetMsgId(NextMsgId)
            .SetBuf(buf).Build();
        /*
        var vec = GenMessage.CreateGenMessage(builder, type, builder.CreateString(UserInfo.Instance.Id), NextMsgId, builder.CreateBuffVector(GenMessage.StartBufVector, buffSeg), frame);
        builder.Finish(vec.Value);
		//Debug.Log (string.Format("{0}: Send Frame {1} To {2}", UserInfo.Instance.Id, frame, "All"));
        var seg = GenMessage.GetRootAsGenMessage(builder.DataBuffer);
         * */
        MessageDispatcher.Instance.Send(genMessage.ToByteArray());
        if (type != MessageType.GetMissingCmd)
        {
            latestCommands.Enqueue(genMessage);
            FutureFrame = frame;
        }
    }

    public void RequestRpc(int frame, UdpNetBehaviour beh, string methodName, ArraySegment<byte> seg)
    {
        /*
        var builder = new FlatBufferBuilder(1);
        var vec = RpcMsg.CreateRpcMsg(builder, beh.NetId, builder.CreateString(methodName), builder.CreateBuffVector(RpcMsg.StartArgbufVector, seg));
        builder.Finish(vec.Value);
        var dataBuffer = builder.DataBuffer;
         * */
        var msg = MsgRpc.CreateBuilder()
            .SetNetId(beh.NetId)
            .SetMethod(methodName)
            .SetArgbuf(ByteString.CopyFrom(seg.Array, seg.Offset, seg.Count)).Build();
        Request(MessageType.Rpc, frame, msg);
    }

    public void RequestEmpty(int frame)
    {
        Request(MessageType.Empty, frame, (IMessage)null);
    }

    public void Instantiate(string path, Vector3 pos, Quaternion rotation)
    {
        FrameController.Instance.RegisterCurrentCommand();
        RequestCreateObj(FrameController.Instance.GetExecuteFrame, path, pos, rotation);
    }
    public void RequestCreateObj(int frame, string path, Vector3 pos, Quaternion rotation)
    {
        /*
        var builder = new FlatBufferBuilder(1);
        CreateObj.StartCreateObj(builder);
        CreateObj.AddPath(builder, builder.CreateString(path));
        CreateObj.AddPos(builder, Vec3.CreateVec3(builder, pos.x, pos.y, pos.z));
        CreateObj.AddRot(builder, Quat.CreateQuat(builder, rotation.x, rotation.y, rotation.z, rotation.w));
        var vec = CreateObj.EndCreateObj(builder);
        builder.Finish(vec.Value);
        var dataBuffer = builder.DataBuffer;
         * */
        var msg = MsgCreateObj.CreateBuilder()
            .SetPath(path)
            .SetPos(Vec3.CreateBuilder().SetX(pos.x).SetY(pos.y).SetZ(pos.z).Build())
            .SetRot(Quat.CreateBuilder().SetX(rotation.x).SetY(rotation.y).SetZ(rotation.z).SetW(rotation.w)).Build();
        Request(MessageType.CreateObj, frame, msg);
    }

    public void ReadyForGame()
    {
        FrameController.Instance.RegisterCommand(-1);
        var msg = ByteString.CopyFrom(UserInfo.Instance.Room.id, System.Text.Encoding.UTF8);
        Request(MessageType.ReadyForGame, FrameController.ExecuteFrame, msg);
    }

    public void RequestMissingMsg(int frame, string player)
    {
        Request(MessageType.GetMissingCmd, frame, ByteString.CopyFrom(player, System.Text.Encoding.UTF8));
    }
    public void InvokeRpc(MsgRpc msg)
    {
        behs[msg.NetId].InvokeRpc(msg);
    }

    public void ResendMessage(int frame)
    { 
        GenMessage msg;
        if(latestCommands.TryGetEnum(x => x.Frame == frame, out msg)){
			var newMsgId = NextMsgId;
			MessageDispatcher.Instance.Send(msg.ToByteArray());
        }
    }
}
