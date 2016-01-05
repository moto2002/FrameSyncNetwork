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
#if TEST
    int bytesSend = 0;
    int bytesReceived = 0;
    float startTime = 0;
    bool GameStarted = false;
    int packSize = 0;
#endif

    public void Send(System.ArraySegment<byte> byteSeg)
    {
#if TEST
        bytesSend += byteSeg.Count;
#endif
		client.Send (byteSeg.Array, byteSeg.Offset, byteSeg.Count);
    }

    void OnMessageReceived(byte[] bytes)
    {
#if TEST
        if (!GameStarted)
        {
            GameStarted = true;
            startTime = Time.realtimeSinceStartup;
        }
        if (packSize < bytes.Length)
        {
            packSize = bytes.Length;
        }
        bytesReceived += bytes.Length;
#endif
        GenMessage msg = GenMessage.GetRootAsGenMessage(new ByteBuffer(bytes, 0));
        var buffSeg = msg.GetBufBytes().Value;
        ByteBuffer bb = new ByteBuffer(buffSeg.Array, buffSeg.Offset);
        actionDict[msg.MsgType](msg.Frame, msg.PId, bb);
    }
#if TEST
    void OnGUI()
    {

        if (GameStarted)
        {
            float et = Time.realtimeSinceStartup - startTime;
            GUILayout.Button(string.Format("Avg  In: {0:0.00}KB/S,   Out: {1:0.00}KB/S, packSize: {2}", bytesReceived / et / 1024, bytesSend / et / 1024, packSize));
            if (et > 10) {
                startTime = Time.realtimeSinceStartup;
                bytesSend = 0;
                bytesReceived = 0;
                packSize = 0;
            }
        }
    }
#endif
    public void RegisterMsgType(MessageType type, System.Action<int, string, ByteBuffer> action)
    {
        actionDict[type] = action;
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
