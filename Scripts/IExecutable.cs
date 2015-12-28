using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IExecutable
{
    void Execute();
    int Frame
    {
        get;
    }
}


public abstract class ExecutableCmd<T> : IExecutable
{
    protected int frame;
    public abstract void Execute();
    public ExecutableCmd(int frame){
        this.frame = frame;
    }
    public ExecutableCmd(int frame, T msg)
    {
        this.frame = frame;
    }

    public int Frame
    {
        get {
            return frame;
        }
    }
}

public class RpcExeObj : ExecutableCmd<Messages.RpcMsg>
{
    Messages.RpcMsg msg;
    public override void Execute()
    {
        UdpNetManager.Instance.InvokeRpc(msg);
    }

    public RpcExeObj(int frame, Messages.RpcMsg msg) 
        : base(frame, msg)
    {
        this.msg = msg;
    }
}

public class CreateObjCmd : ExecutableCmd<Messages.CreateObj>
{
    Vector3 pos;
    Quaternion rot;
    string path;
    private CreateObjCmd(int frame) : base(frame)
    { }
    public CreateObjCmd(int frame, Messages.CreateObj msg)
        : base(frame, msg)
    {
        var _pos = msg.Pos;
        var _rot = msg.Rot;
        pos = new Vector3(_pos.X, _pos.Y, _pos.Z);
        rot = new Quaternion(_rot.X, _rot.Y, _rot.Z, _rot.W);
        path = msg.Path;
    }

    public static CreateObjCmd CreatePlayer(int frame, string playerPrefab)
    {
        return new CreateObjCmd(frame) { pos = Vector3.zero, rot = Quaternion.identity, path = playerPrefab};
    }
    public override void Execute()
    {
        GameObject.Instantiate(Resources.Load(path), pos, rot);
    }
}
