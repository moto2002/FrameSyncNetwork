using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Messages;
using FlatBuffers;
using System.Collections.Generic;
public class MessageDispatcher : MonoBehaviour
{
    Dictionary<MessageType, System.Action<int, string, ByteBuffer>> actionDict = new Dictionary<MessageType, System.Action<int, string, ByteBuffer>>();
    GenUdpClient client;
    public static MessageDispatcher Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this);
        }
        Application.runInBackground = true;
    }

    void Start()
    {
        Connect();
    }

    void Connect()
    {
        client = new GenUdpClient("127.0.0.1", 2015);
        client.OnReceiveMessage = OnMessageReceived;
        client.Start(this);
    }

    public void Send(ByteBuffer bf)
    { 
        client.Send(bf.Data, bf.Position, bf.Length - bf.Position);
    }

    void OnMessageReceived(byte[] bytes)
    {
        GenMessage msg = GenMessage.GetRootAsGenMessage(new ByteBuffer(bytes, 0));
        byte[] msgBuf = msg.GetBufBytes().Value.Array;
        ByteBuffer bb = new ByteBuffer(msgBuf, 0);
        actionDict[msg.MsgType](msg.Frame, msg.PId, bb);
    }

    

    public void RegisterMsgType(MessageType type, System.Action<int, string, ByteBuffer> action)
    {
        actionDict[type] = action;
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
