using UnityEngine;
using System.Collections.Generic;

public class NetPlayer {
    public int EnterFrame
    {
        get;
        set;
    }

    Queue<IExecutable> commandQue = new Queue<IExecutable>();
    public string pId;
    public bool IsReady(int frame)
    {
        if (frame < EnterFrame)
            return true;
        if (commandQue.Count == 0)
            return false;
        var cmd = commandQue.Peek();
        if (cmd.Frame == frame)
            return true;
        return false;
    }

    public void GetCommand(IExecutable cmd)
    {
        commandQue.Enqueue(cmd);
    }

    public void ProcessCommand(int frame)
    {
        while(commandQue.Count > 0){
            var cmd = commandQue.Peek();
            if (cmd.Frame == frame)
            {
                commandQue.Dequeue();
                cmd.Execute();
            }
            else if (cmd.Frame > frame)
                break;
            else
                Debug.LogError("Frame Error");
        }
    }
}
