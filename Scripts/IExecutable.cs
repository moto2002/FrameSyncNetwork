using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utility;
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

public class EmptyObj : IExecutable
{
    public int Frame
    {
        get;
        private set;
    }
    public EmptyObj(int frame)
    {
        this.Frame = frame;
    }

    public void Execute()
    {
        
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
    GameObject prefab;
    string player;
    private CreateObjCmd(int frame, string player) : base(frame)
    {
        this.player = player;
    }
    public CreateObjCmd(int frame, string player, Messages.CreateObj msg)
        : base(frame, msg)
    {
        var _pos = msg.Pos;
        var _rot = msg.Rot;
        pos = new Vector3(_pos.X, _pos.Y, _pos.Z);
        rot = new Quaternion(_rot.X, _rot.Y, _rot.Z, _rot.W);
        path = msg.Path;
        this.player = player;
    }

    public static CreateObjCmd CreatePlayer(int frame, string player, GameObject playerPrefab)
    {
        return new CreateObjCmd(frame, player) { pos = Vector3.zero, rot = Quaternion.identity, prefab = playerPrefab };
    }
    public override void Execute()
    {
        GameObject go;
        if (prefab != null)
            go = GameObject.Instantiate(prefab, pos, rot) as GameObject;
        else
            go = GameObject.Instantiate(Resources.Load(path), pos, rot) as GameObject;
        go.GetUdpNetwork().ownerId = player;
        var behaviours = go.GetComponentsInChildren<FrameBehaviour>(false);
        for (int i = 0; i < behaviours.Length; i++) {
            behaviours[i].Init();
        }
    }
}
