using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Messages;
using FlatBuffers;
using System.Collections.Generic;
public class MessageDispatcher : MonoBehaviour
{
    public string Ip = "127.0.0.1";
    public int Port = 2015;
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

    public void Connect()
    {
        client = new GenUdpClient(Ip, Port);
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
		//Debug.Log(string.Format("RoomMsg: {0}, Frame: {1}", msg.MsgType, msg.Frame));
        var buffSeg = msg.GetBufBytes().Value;
        ByteBuffer bb = new ByteBuffer(buffSeg.Array, buffSeg.Offset);
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
