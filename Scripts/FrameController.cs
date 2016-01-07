using UnityEngine;
using System.Collections.Generic;

public class FrameController : AutoCreateSingleTon<FrameController> {
    public const int ExecuteFrame = 2;
    public const int PushBack = 1;
    float lastUpdateTime;

    public int GetExecuteFrame {
        get {
            int next = Frame + 1;
            if (next % ExecuteFrame == 0) {
				return next + PushBack * ExecuteFrame;
            }
            return next + (ExecuteFrame - next % ExecuteFrame) + PushBack * ExecuteFrame;
        }
    }

	EmptyCommandEmiter ece = new EmptyCommandEmiter();

    public void RegisterCurrentCommand() {
        ece.AddCommand(Frame);
    }

    public void RegisterCommand(int frame)
    {
        ece.AddCommand(frame);
    }
    public float DeltaTime
    {
        get;
        private set;
    }
    public bool Freezed {
		get;
		set;
    }
    public int Frame
    {
        get;
        private set;
    }
    List<FrameBehaviour> behaviours = new List<FrameBehaviour>();
    List<FrameBehaviour> backUp = new List<FrameBehaviour>();
    List<NetPlayer> players = new List<NetPlayer>();

    public NetPlayer AddPlayer(string id, int frame)
    {
        var ret = new NetPlayer() { pId = id , EnterFrame = frame};
        players.Add(ret);
        return ret;
    }

    public int AlreadyPlayerCount {
        get {
            return players.Count;
        }
    }
    public NetPlayer GetPlayer(string id)
    {
        return players.Find(x => x.pId == id);
    }
    public void AddBehaviour(FrameBehaviour item)
    {
        behaviours.Add(item);
    }
	// Use this for initialization
	void Start () {
        DeltaTime = 0.02f;
	}

    public void StartFrameLoop()
    {
        lastUpdateTime = Time.realtimeSinceStartup;
    }
	// Update is called once per frame
	void Update () {
        if (Freezed)
            return;
        while (Time.realtimeSinceStartup - lastUpdateTime > DeltaTime) {
            FrameUpdate();
            lastUpdateTime += DeltaTime;
        }
	}

    void FrameUpdate()
    {
		ece.Update(Frame);
        if (Frame % ExecuteFrame == 0)
        {
            if (AllReady()) {
                players.ForEach(x => x.ProcessCommand(Frame));
                Step();
            }
        }
        else {
            Step();
        }
    }

    void Step()
    {
        backUp.Clear();
        for (int i = 0, iMax = behaviours.Count; i < iMax; i++)
        {
            var item = behaviours[i];
            if (!item.IsDisposed)
            {
                item.FrameUpdate();
                backUp.Add(item);
            }
        }
        var t = behaviours;
        behaviours = backUp;
        backUp = t;
        Frame++;
    }

    void ProcessCommands()
    { 
    
    }

    bool AllReady()
    {
        for (int i = 0, iMax = players.Count; i < iMax; i++)
        {
            var item = players[i];
            if (!item.IsReady(Frame))
            {
                item.IncreaseFrameLockTime(Frame);
                if (item.lockTime > NetPlayer.MaxWaitFrame) {
                    item.RequestMissingCmd();
                }
                return false;
            }
        }
        return true;
    }
}
