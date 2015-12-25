using UnityEngine;
using System.Collections;

public abstract class FrameBehaviour : MonoBehaviour {
    public bool IsDisposed {
        get;
        private set;
    }
	// Use this for initialization
    void Awake()
    { 
    
    }
	void Start () {
	
	}
    protected virtual void OnInit()
    {
        FrameController.Instance.AddBehaviour(this);
    }
    protected virtual void OnDestroy()
    {
        IsDisposed = true;
    }

    public void  Init()
    {
        OnInit();
    }

    // Update is called once per frame
    public abstract void FrameUpdate();
}
