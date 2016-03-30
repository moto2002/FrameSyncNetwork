using UnityEngine;
using System.Collections.Generic;

public class NetPlayer {
    public NetPlayer()
    {
        lockFrame = -1;
        lockTime = 0;
    }
    public const int MaxWaitFrame = 20;
    Queue<IExecutable> commandQue = new Queue<IExecutable>();
    Queue<IExecutable> backQue = new Queue<IExecutable>();
    public int EnterFrame
    {
        get;
        set;
    }
    public int pIdx;
    public int lockFrame
    {
        get;
        private set;
    }
    public int lockTime
    {
        get;
        private set;
    }
    public bool IsReady(int frame)
    {
		if (frame == 0)
			return true;
        if (commandQue.Count == 0)
            return false;
        var cmd = commandQue.Peek();
        if (cmd.Frame == frame)
            return true;
        return false;
    }

    public void IncreaseFrameLockTime(int frame)
    {
        lockFrame = frame;
        lockTime += 1;
    }

    public void RequestMissingCmd()
    {
        if (lockFrame == -1)
            return;
        if (pIdx == UserInfo.Instance.Index)
        {
            UdpNetManager.Instance.ResendMessage(lockFrame);
        }
        else
        {
            UdpNetManager.Instance.RequestMissingMsg(lockFrame, pIdx);
        }
        lockFrame = -1;
        lockTime = 0;
    }

    public void GetCommand(IExecutable cmd)
    {
        if (cmd.Frame < FrameController.Instance.Frame)
            return;
        backQue.Clear();
        while (commandQue.Count > 0)
        {
            var peek = commandQue.Peek();
            if (peek.Frame < cmd.Frame)
                backQue.Enqueue(commandQue.Dequeue());
            else
                break;
        }
        if (commandQue.Count > 0)
        {
            var peek = commandQue.Peek();
            if (peek.Frame == cmd.Frame)
            {
                commandQue.Dequeue();
            }
        }
        backQue.Enqueue(cmd);
        while (commandQue.Count > 0)
        {
            backQue.Enqueue(commandQue.Dequeue());
        }
        var t = commandQue;
        commandQue = backQue;
        backQue = t;
    }

    public void ProcessCommand(int frame)
    {
        while(commandQue.Count > 0){
            var cmd = commandQue.Peek();
            if (cmd.Frame == frame)
            {
                commandQue.Dequeue();
				if(!(cmd is EmptyObj))
                	cmd.Execute();
            }
            else if (cmd.Frame > frame)
                break;
            else
                Debug.LogError("Frame Error");
        }
    }
}
