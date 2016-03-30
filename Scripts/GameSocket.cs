using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.IO;
public class GameSocket : System.IDisposable
{
    public System.Action<byte[]> onReceiveMessage;
    public System.Action onConnected;
    public System.Action onDisconnect;
    private Queue<byte[]> receiveMsgQue = new Queue<byte[]>();
    private TcpClient client;
    private string host;
    private int port;

    public void Flush()
    {
        client.GetStream().Flush();
    }
    public Stream GetStream()
    {
        return client.GetStream();
    }
    public bool Error
    {
        get;
        private set;
    }
    public bool Connected
    {
        get;
        private set;
    }

    public GameSocket(string host, int port)
    {
        Connected = false;
        client = new TcpClient();
        this.host = host;
        this.port = port;
    }

    public void Connect()
    {
        client.BeginConnect(host, port, ConnectCallback, null);
    }

    void ConnectCallback(System.IAsyncResult res)
    {
        try
        {
            client.EndConnect(res);
            Connected = true;
            if (onConnected != null)
            {
                onConnected();
            }
            new Thread(new ThreadStart(RecvLoop)).Start();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.StackTrace);
            Error = true;
        }
    }

    public void Send(System.ArraySegment<byte> bytes)
    {
        if (!Connected)
            return;
        var stream = client.GetStream();
        int len = bytes.Count;
        stream.WriteByte((byte)(len & 0xFF));
        stream.WriteByte((byte)(len >> 8 & 0xFF));
        stream.WriteByte((byte)(len >> 16 & 0xFF));
        stream.WriteByte((byte)(len >> 24 & 0xFF));
        stream.Write(bytes.Array, bytes.Offset, len);
        stream.Flush();
    }

    public IEnumerator Dispatcher()
    {
        while (Connected)
        {
            yield return null;
            lock (receiveMsgQue)
            {
                while (receiveMsgQue.Count > 0)
                {
                    var msg = receiveMsgQue.Dequeue();
                    if (onReceiveMessage != null)
                    {
                        onReceiveMessage(msg);
                    }
                }
            }
        }
        if (onDisconnect != null)
            onDisconnect();
    }

    void RecvLoop()
    {
        var stream = client.GetStream();
        byte[] len_buf = new byte[4];
        while (Connected)
        {
            if (!ReadStream(stream, len_buf, 0, 4))
            {
                break;
            }
            int msgLen = System.BitConverter.ToInt32(len_buf, 0);
            byte[] msgbuf = new byte[msgLen];
            if (!ReadStream(stream, msgbuf, 0, msgLen))
            {
                break;
            }
            lock (receiveMsgQue)
            {
                receiveMsgQue.Enqueue(msgbuf);
            }
        }
        Debug.Log("ReceiveLoop Quited");
        Connected = false;
    }

    static bool ReadStream(NetworkStream stream, byte[] buf, int offset, int count)
    {
        int sum = 0;
        bool ok = true;

        while (sum < count)
        {
            try
            {
                int len = stream.Read(buf, offset + sum, count - sum);
                sum += len;
            }
            catch
            {
                ok = false;
                break;
            }
        }
        return ok;
    }

    public void Dispose()
    {
        Connected = false;
        client.Close();
    }
}