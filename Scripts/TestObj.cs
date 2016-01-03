using UnityEngine;
using System.Collections;
using Utility;

[UseNetBehaviour]
public class TestObj : FrameBehaviour
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
    static TestObj _instance;

    public static TestObj Instance {
        get {
            return _instance;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        if (this.GetUdpNetwork().ownerId == UserInfo.Instance.Id)
        {
			_instance = this;
        }
    }

	void OnGUI()
	{
		if (this.GetUdpNetwork ().ownerId == UserInfo.Instance.Id) {
			//GUILayout.Label (UserInfo.Instance.Id);
		}
	}

    [UdpRpc]
    public void MoveTo(Vector3 pos)
    {
        transform.position = pos;
    }

    [UdpRpc]
    public void WalkTo(Vector3 pos)
    {
        elapsed = 0;
        from = transform.position;
        to = pos;
        total = (to - from).magnitude / speed;
        state = State.moving;
    }

    [UdpRpc]
    public void TurnColor(Color c)
    {
        GetComponent<Renderer>().material.color = c;
    }

    [UdpRpc]
    public void LogString(string str)
    {
        Debug.Log(str);
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
