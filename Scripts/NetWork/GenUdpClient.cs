using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class GenUdpClient : IDisposable
{
    public bool IsDisposed
    {
        get;
        private set;
    }
    IPEndPoint endPoint;
    private UdpClient client;
    public Action<byte[]> OnReceiveMessage;
    public Queue<byte[]> msgQue = new Queue<byte[]>();
    public void Dispose()
    {
        IsDisposed = true;
        client.Close();
    }

    public GenUdpClient(string ip, int port)
    {
        client = new UdpClient(ip, port);
        endPoint = new IPEndPoint(IPAddress.Any, port);
    }

    public void Start(UnityEngine.MonoBehaviour comp)
    {
        client.BeginReceive(RecvAsync, null);
        comp.StartCoroutine(Dispatcher());
    }

    void RecvAsync(IAsyncResult res)
    { 
        var bytes = client.EndReceive(res, ref endPoint);
        lock(msgQue){
            msgQue.Enqueue(bytes);
        }
        if (!IsDisposed) {
            client.BeginReceive(RecvAsync, null);
        }
    }

    IEnumerator Dispatcher()
    {
        while (!IsDisposed)
        {
            yield return null;
            if (OnReceiveMessage != null)
            {
                lock (msgQue)
                {
                    while (msgQue.Count > 0)
                    {
                        var bytes = msgQue.Dequeue();
                        OnReceiveMessage(bytes);
                    }
                }
            }
        }
    }

    public void Send(byte[] bytes)
    {
        client.Send(bytes, bytes.Length);
    }

    public void Send(byte[] buf, int offset, int count)
    {
        client.Client.Send(buf, offset, count, SocketFlags.None);
    }
}