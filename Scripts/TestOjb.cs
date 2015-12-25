using UnityEngine;
using System.Collections;

[UseNetBehaviour]
public class TestOjb : FrameSingleton<TestOjb>
{
    enum State{
        idle,
        moving
    }
    State state = State.idle;
    float elapsed, total;
    float speed = 10;
    Vector3 from, to;

    // Use this for initialization
    // Update is called once per frame

    [UdpRpc(rpcMsgType = typeof(Vector3))]
    public void MoveTo(Vector3 pos)
    {
        transform.position = pos;
    }

    [UdpRpc(rpcMsgType = typeof(Vector3))]
    public void WalkTo(Vector3 pos)
    {
        elapsed = 0;
        from = transform.position;
        to = pos;
        total = (to - from).magnitude / speed;
        state = State.moving;
    }

    [UdpRpc(rpcMsgType = typeof(Color))]
    public void TurnColor(Color c)
    {
        GetComponent<Renderer>().material.color = c;
    }

    public override void FrameUpdate()
    {
        switch (state)
        {
            case State.idle:
                break;
            case State.moving:
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(from, to, elapsed / total);
                if (elapsed > total)
                {
                    state = State.idle;
                }
                break;
        }
    }
}
