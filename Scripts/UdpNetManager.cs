using UnityEngine;
using Messages;
using FlatBuffers;
using System.Collections.Generic;
using Utility;
public  class UdpNetManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private int nId = 0;
    private int mId = 0;
    private static UdpNetManager m_instance;
    private List<UdpNetBehaviour> behs = new List<UdpNetBehaviour>();
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
        FrameController.Instance.Freezed = true;
        MessageDispatcher.Instance.RegisterMsgType(MessageType.Rpc, OnRpcMsgCallback);
        MessageDispatcher.Instance.RegisterMsgType(MessageType.CreateObj, OnCreateObjCallback);
        MessageDispatcher.Instance.RegisterMsgType(MessageType.AddPlayer, OnAddPlayerCallback);
        MessageDispatcher.Instance.RegisterMsgType(MessageType.Empty, OnEmptyCallBack);
        MessageDispatcher.Instance.Connect();
        RequestEnterRoom();
    }

    void OnDestroy()
    {
        m_instance = null;
    }
    void OnEmptyCallBack(int frame, string pId, ByteBuffer bb)
    {
        FrameController.Instance.GetPlayer(pId).GetCommand(new EmptyObj(frame));
    }
    void OnRpcMsgCallback(int frame, string pId, ByteBuffer bb)
    {
        var msg = RpcMsg.GetRootAsRpcMsg(bb);
        FrameController.Instance.GetPlayer(pId).GetCommand(new RpcExeObj(frame, msg));
    }

    void OnCreateObjCallback(int frame, string pId, ByteBuffer bb)
    {
        var msg = CreateObj.GetRootAsCreateObj(bb);
        FrameController.Instance.GetPlayer(pId).GetCommand(new CreateObjCmd(frame, msg));
    }

    void OnAddPlayerCallback(int frame, string pId, ByteBuffer bb)
    {
        FrameController.Instance.AddPlayer(pId, frame).GetCommand(CreateObjCmd.CreatePlayer(frame, playerPrefab));
        if (pId == UserInfo.Instance.Id) {
            FrameController.Instance.Freezed = false;
            FrameController.Instance.StartFrameLoop();
        }
    }
    void Request(MessageType type, int frame, System.ArraySegment<byte> buffSeg)
    {
        FlatBufferBuilder builder = new FlatBufferBuilder(1);
        var vec = GenMessage.CreateGenMessage(builder, type, builder.CreateString(UserInfo.Instance.Id), NextMsgId, builder.CreateBuffVector(GenMessage.StartBufVector, buffSeg), frame);
        builder.Finish(vec.Value);
        MessageDispatcher.Instance.Send(builder.DataBuffer);
    }

    public void RequestRpc(int frame, UdpNetBehaviour beh, string methodName, byte[] argBuf)
    {
        var builder = new FlatBufferBuilder(1);
        var vec = RpcMsg.CreateRpcMsg(builder, beh.NetId, builder.CreateString(methodName), RpcMsg.CreateArgbufVector(builder, argBuf));
        builder.Finish(vec.Value);
        var dataBuffer = builder.DataBuffer;
        Request(MessageType.Rpc, frame, dataBuffer.GetArraySegment());
    }

    public void RequestEmpty(int frame)
    {
        Request(MessageType.Empty, frame, new System.ArraySegment<byte>(new byte[0]));
    }

    public void Instantiate(string path, Vector3 pos, Quaternion rotation)
    {
        FrameController.Instance.RegisterCommand();
        RequestCreateObj(FrameController.Instance.GetExecuteFrame, path, pos, rotation);
    }
    public void RequestCreateObj(int frame, string path, Vector3 pos, Quaternion rotation)
    {
        var builder = new FlatBufferBuilder(1);
        CreateObj.StartCreateObj(builder);
        CreateObj.AddPath(builder, builder.CreateString(path));
        CreateObj.AddPos(builder, Vec3.CreateVec3(builder, pos.x, pos.y, pos.z));
        CreateObj.AddRot(builder, Quat.CreateQuat(builder, rotation.x, rotation.y, rotation.z, rotation.w));
        var vec = CreateObj.EndCreateObj(builder);
        builder.Finish(vec.Value);
        var dataBuffer = builder.DataBuffer;
        Request(MessageType.CreateObj, frame, dataBuffer.GetArraySegment());
    }

    public void RequestEnterRoom()
    {
        //FrameController.Instance.RegisterCommand();
        Request(MessageType.AddPlayer, 0, new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(UserInfo.Instance.RoomId)));
    }
    public void InvokeRpc(RpcMsg msg)
    {
        behs[msg.NetId].InvokeRpc(msg);
    }
}
