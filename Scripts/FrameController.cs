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
                return next + PushBack;
            }
            return next + (ExecuteFrame - next % ExecuteFrame) + PushBack * ExecuteFrame;
        }
    }

    EmptyCommandEmiter ece;

    public void RegisterCommand() {
        if (ece == null) {
            ece = new EmptyCommandEmiter();
        }
        ece.AddCommand(Frame);
    }
    public float DeltaTime
    {
        get;
        private set;
    }
    public bool Freezed
    {
        get;
        private set;
    }
    public int Frame
    {
        get;
        private set;
    }
    List<FrameBehaviour> behaviours = new List<FrameBehaviour>();
    List<FrameBehaviour> backUp = new List<FrameBehaviour>();
    List<NetPlayer> players = new List<NetPlayer>();

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
	
	// Update is called once per frame
	void Update () {
        while (Time.realtimeSinceStartup - lastUpdateTime > DeltaTime) {
            FrameUpdate();
            lastUpdateTime += DeltaTime;
        }
	}

    void FrameUpdate()
    {
        if (!Freezed)
        {
            if (Frame >= ExecuteFrame && Frame % ExecuteFrame == 0)
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

    bool AllReady()
    {
        for (int i = 0, iMax = players.Count; i < iMax; i++)
        {
            if (!players[i].IsReady(Frame))
                return false;
        }
        return true;
    }
}
