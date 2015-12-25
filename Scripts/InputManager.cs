using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	// Use this for initialization
    Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos;
            if (GetHitPos(out pos))
            {
                TestOjb.Instance.GetComponent<UdpNetBehaviour>().RPC("MoveTo", RPCMode.All, pos);
            }
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos;
            if (GetHitPos(out pos))
            {
                TestOjb.Instance.GetComponent<UdpNetBehaviour>().RPC("WalkTo", RPCMode.All, pos);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            int index = Random.Range(0, colors.Length);
            TestOjb.Instance.GetComponent<UdpNetBehaviour>().RPC("TurnColor", RPCMode.All, colors[index]);
        }
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
