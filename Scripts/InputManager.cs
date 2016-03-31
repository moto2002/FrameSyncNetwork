using UnityEngine;
using System.Collections;
using Utility;
public enum MovingState
{
    Idle,
    Up,
    Down
}
public class InputManager : AutoCreateSingleTon<InputManager> {
    public MovingState movingState
    {
        get;
        private set;
    }
    public bool IsShotting
    {
        get;
        private set;
    }
	// Use this for initialization
    Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            movingState = MovingState.Up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movingState = MovingState.Down;
        }
        else {
            movingState = MovingState.Idle;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            IsShotting = true;
        }
        else {
            IsShotting = false;
        }
        /*
        if (FrameController.Instance.Frame < FrameController.ExecuteFrame)
            return;
        int exeFrame = FrameController.Instance.GetExecuteFrame;
        if (exeFrame == UdpNetManager.Instance.FutureFrame)
            return;
        if (TestObj.Instance == null)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos;
            if (GetHitPos(out pos))
            {
                TestObj.Instance.GetUdpNetwork().RPC("MoveTo", RPCMode.All, pos);
            }
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos;
            if (GetHitPos(out pos))
            {
                TestObj.Instance.GetUdpNetwork().RPC("WalkTo", RPCMode.All, pos);
            }
            return;
        }


        if (Input.GetKeyDown(KeyCode.S)) {
            TestObj.Instance.GetUdpNetwork().RPC("LogString", RPCMode.All, "Hello World");
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (!TestObj.Instance.IsJumping)
            {
                TestObj.Instance.GetUdpNetwork().RPC("Jump", RPCMode.All);
                return;
            }
        }
         * */
	}
    bool GetHitPos(out Vector3 pos)
    {
        var mp = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mp);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Plane")))
        {
            pos = hit.point;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }
}
