using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using WorldMessages;
using FlatBuffers;
public class WorldNetwork : MonoBehaviour {
    sealed class WaitReplyItem
    {
        public int msgId;
        public Action<byte[]> action;
    }
    Queue<WaitReplyItem> callBackQue = new Queue<WaitReplyItem>();
    Dictionary<string, Action<byte[]>> pushCallbackDict = new Dictionary<string, Action<byte[]>>();
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
        client = new GenUdpClient("127.0.0.1", 2015);
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
            Action<byte[]> action;
            if (pushCallbackDict.TryGetValue(msg.Type.ToString(), out action))
            {
                action(bytes);
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
                    peek.action(msg.GetBuffBytes().Value.Array);
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
    public void Send<ReplyType>(MessageType type, ByteBuffer bb, Action<ReplyType> callBack)
    {
        int mId = MsgId;
        FlatBufferBuilder fb = new FlatBufferBuilder(1);
        var bytes = Utility.DataUtility.GetDataBuffer(bb.Length - bb.Position, bb.Get, bb.Position);
        var vec = WorldMessage.CreateWorldMessage(fb, type, fb.CreateString(UserInfo.Instance.id), WorldMessage.CreateBuffVector(fb, bytes), mId);
        fb.Finish(vec.Value);
        Action<byte[]> action = (recvBytes) =>
        {
            ReplyType reply = DataPaser.Instance.Deserialize<ReplyType>(recvBytes);
            callBack(reply);
        };
        callBackQue.Enqueue(new WaitReplyItem() { msgId = mId, action = action});
    }
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        client.Dispose();
    }
}
