using UnityEngine;
using System.Collections;
using Utility;

[UseNetBehaviour]
public class TestObj : FrameBehaviour
{
    enum State{
        idle,
        moving,
        jumping
    }
    public float jumpSpeed = 10.0f;
    State state = State.idle;
    float elapsed, total;
    float speed = 10;
    Vector3 from, to;
    float jumpTime;
    Vector3 jumpPos;

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

    public bool IsJumping {
        get {
            return state == State.jumping;
        }
    }
    [UdpRpc]
    public void MoveTo(Vector3 pos)
    {
        if (state == State.jumping)
            return;
        transform.position = pos;
    }

    [UdpRpc]
    public void WalkTo(Vector3 pos)
    {
        if (state == State.jumping)
            return;
        elapsed = 0;
        from = transform.position;
        to = pos;
        total = (to - from).magnitude / speed;
        state = State.moving;
    }


    [UdpRpc]
    public void LogString(string str)
    {
        Debug.Log(str);
    }

    [UdpRpc]
    public void Jump()
    {
        if (state == State.jumping)
            return;
        elapsed = 0;
        jumpTime = 2 * jumpSpeed / Physics.gravity.magnitude;
        jumpPos = transform.position;
        state = State.jumping;
    }

    public override void FrameUpdate()
    {
        switch (state)
        {
            case State.idle:
                break;
            case State.moving:
                elapsed += FrameController.Instance.DeltaTime;
                transform.position = Vector3.Lerp(from, to, elapsed / total);
                if (elapsed > total)
                {
                    state = State.idle;
                }
                break;
            case State.jumping:
                elapsed += FrameController.Instance.DeltaTime;
                if (elapsed > jumpTime)
                {
                    state = State.idle;
                    transform.position = jumpPos;
                }
                else {
                    transform.position = jumpPos + Vector3.up * (jumpSpeed * elapsed - Physics.gravity.magnitude * elapsed * elapsed / 2);
                }
                break;
        }
    }
}
