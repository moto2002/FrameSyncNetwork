using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using WorldMessages;
using FlatBuffers;
using Utility;
public class WorldNetwork : AutoCreateSingleTon<WorldNetwork> {
    public string RoomSceneName;
    public string Ip = "127.0.0.1";
    public int port = 2015;
    sealed class WaitReplyItem
    {
        public int msgId;
        public Action<ArraySegment<byte>> action;
    }
    Queue<WaitReplyItem> callBackQue = new Queue<WaitReplyItem>();
    Dictionary<string, Action<ArraySegment<byte>>> pushCallbackDict = new Dictionary<string, Action<ArraySegment<byte>>>();
    GenUdpClient client;
    private int msgId;
    int MsgId
    {
        get {
            return msgId++; ;
        }
    }
	// Use this for initialization
	void Start () {
        client = new GenUdpClient(Ip, port);
        client.OnReceiveMessage = OnRecvMessage;
        msgId = 1;
        client.Start(this);
	}

    public void RegistPushMsgCallback<T>(System.Action<T> callBack)
    {
        pushCallbackDict[typeof(T).Name] = (bytes) =>
        {
            T msg = DataPaser.Instance.Deserialize<T>(bytes);
            callBack(msg);
        };
    }

    void OnRecvMessage(byte[] bytes)
    {
        ReplyMsg msg = ReplyMsg.GetRootAsReplyMsg(new FlatBuffers.ByteBuffer(bytes, 0));
        var recvMsgId = msg.MsgId;
        if (recvMsgId == -1)
        {
            Action<ArraySegment<byte>> action;
            if (pushCallbackDict.TryGetValue(msg.Type.ToString(), out action))
            {
                var seg = msg.GetBuffBytes().Value;
                action(seg);
            }
            else {
                Debug.LogWarning(string.Format("MsgType {0} not registered", msg.Type));
            }
        }
        else
        {
            if (callBackQue.Count == 0)
            {
                Debug.LogError("MsgId Error: " + msg.MsgId);
            }
            else
            {
                var peek = callBackQue.Peek();
                if (peek.msgId == msg.MsgId)
                {
                    callBackQue.Dequeue();
                    peek.action(msg.GetBuffBytes().Value);
                }
                else {
                    Debug.LogWarning(string.Format("MsgId mismatch, peek: {0}, received : {1}", peek.msgId, msg.MsgId));
                }
            }
        }
    }

    public void Send(ByteBuffer bb)
    {
        client.Send(bb.Data, bb.Position, bb.Length - bb.Position);  
    }
    public void Send<ReplyType>(MessageType type, ArraySegment<byte> buffSegment, Action<ReplyType> callBack)
    {
        int mId = MsgId;
        FlatBufferBuilder fb = new FlatBufferBuilder(1);
        var vec = WorldMessage.CreateWorldMessage(fb, type, fb.CreateString(UserInfo.Instance.Id), fb.CreateBuffVector(WorldMessage.StartBuffVector, buffSegment), mId);
        fb.Finish(vec.Value);
        Action<ArraySegment<byte>> action = (recvBytes) =>
        {
            ReplyType reply = DataPaser.Instance.Deserialize<ReplyType>(recvBytes);
            callBack(reply);
        };
        callBackQue.Enqueue(new WaitReplyItem() { msgId = mId, action = action});
        Send(fb.DataBuffer);
    }
	// Update is called once per frame
	void Update () {
	
	}

    protected override void OnDestroy()
    {
        base.OnDestroy();
        client.Dispose();
    }
}
