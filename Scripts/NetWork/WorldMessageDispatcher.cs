using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using world_messages;
using Google.ProtocolBuffers;
using Utility;
public class WorldMessageDispatcher : AutoCreateSingleTon<WorldMessageDispatcher>
{

    public string RoomSceneName;
    public string host = "127.0.0.1";
    public int port = 1234;
    private Dictionary<string, System.Action<ByteString>> actionDict = new Dictionary<string, System.Action<ByteString>>();
    private Queue<System.Action<ByteString>> callbackQue = new Queue<System.Action<ByteString>>();
    private long loginTime;
    private float loginTimeSinceGameStartup;
    private int msgId = 0;
    public int NextMsgId{
        get {
            return msgId++;
        }
    }
    public long CurrentTimeMS
    {
        get
        {
            return loginTime + Mathf.FloorToInt((Time.realtimeSinceStartup - loginTimeSinceGameStartup) * 1000);
        }
    }

    public void SetLoginTime(long loginTime, float timeSecSinceLogin)
    {
        this.loginTime = loginTime;
        this.loginTimeSinceGameStartup = timeSecSinceLogin;
    }

    private GameSocket socket;

    public float TimeSinceLogin
    {
        get
        {
            return 0;
        }
    }
    protected override void Awake()
    {
        if (mInstance != null && mInstance != this)
        {
            Destroy(this);
        }
        else
        {
            mInstance = this;
        }
    }

    public void RegisterCallBack<T>(System.Func<ByteString, T> parseFunc, System.Action<T> callBack)
    {
        actionDict[typeof(T).Name] = (bytes) =>
        {
            T msg = parseFunc(bytes);
            callBack(msg);
        };
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (socket != null)
            socket.Dispose();
    }


    public bool Connectd
    {
        get
        {
            if (socket == null)
                return false;
            return socket.Connected;
        }
    }
    public bool ConnectionError
    {
        get
        {
            if (socket == null)
                return false;
            return socket.Error;
        }
    }
    // Use this for initialization

    void OnReceiveMsg(byte[] bytes)
    {
        var msg = ReplyMsg.ParseFrom(bytes);
        //Debug.Log("receive message, type: " + msg.Type);
        if (msg.MsgId != -1)
        {
            var action = callbackQue.Dequeue();
            action(msg.Buff);
        }
        else
        {
            var action = actionDict["Msg" + msg.Type.ToString()];
            action(msg.Buff);
        }
    }
    public void Connect(System.Action callback = null)
    {
        if (socket != null)
            socket.Dispose();
        StartCoroutine(StartConnect(callback));
    }

    IEnumerator StartConnect(System.Action callback = null)
    {
        socket = new GameSocket(host, port);
        socket.onReceiveMessage = this.OnReceiveMsg;
        socket.onDisconnect = this.OnDisconnected;
        socket.onConnected = this.OnConnected;
        socket.Connect();
        while (!socket.Connected)
        {
            yield return null;
        }
        if (callback != null)
            callback();
        StartCoroutine(socket.Dispatcher());
    }

    void OnConnected()
    {
        Debug.Log("connected", this);
    }

    void OnDisconnected()
    {
        Debug.Log("connection break");
    }

    void Send(byte[] array, int offset, int count)
    {
        var stream = socket.GetStream();
        if (stream.CanWrite)
        {
            stream.BeginWrite(array, offset, count, (s) =>
            {
                stream.EndWrite(s);
            }, null);
        }
    }
    public void Send(byte[] bb)
    {
        this.socket.Send(new System.ArraySegment<byte>(bb));
    }
    public void Send<ReplyType>(MessageType type, ArraySegment<byte> buffSegment, Action<ReplyType> callBack)
    {
        int mId = NextMsgId;
        var msg = WorldMessage.CreateBuilder()
            .SetType(type)
            .SetPlayerId(UserInfo.Instance.Id)
            .SetBuff(ByteString.CopyFrom(buffSegment.Array, buffSegment.Offset, buffSegment.Count))
            .SetMsgId(mId)
            .Build();
        Action<ByteString> action = (recvBytes) =>
        {
            ReplyType reply = DataPaser.Instance.Deserialize<ReplyType>(recvBytes);
            callBack(reply);
        };
        callbackQue.Enqueue(action);
        Send(msg.ToByteArray());
    }
}
