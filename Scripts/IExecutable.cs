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
public class RpcExeObj : ExecutableCmd<messages.MsgRpc>
{
    messages.MsgRpc msg;
    public override void Execute()
    {
        UdpNetManager.Instance.InvokeRpc(msg);
    }

    public RpcExeObj(int frame, messages.MsgRpc msg) 
        : base(frame, msg)
    {
        this.msg = msg;
    }
}

public class CreateObjCmd : ExecutableCmd<messages.MsgCreateObj>
{
    Vector3 pos;
    Quaternion rot;
    string path;
    GameObject prefab;
    int playerIndex;
    private CreateObjCmd(int frame, int playerIndex) : base(frame)
    {
        this.playerIndex = playerIndex;
    }
    public CreateObjCmd(int frame, int playerIndex, messages.MsgCreateObj msg)
        : base(frame, msg)
    {
        var _pos = msg.Pos;
        var _rot = msg.Rot;
        pos = new Vector3(_pos.X, _pos.Y, _pos.Z);
        rot = new Quaternion(_rot.X, _rot.Y, _rot.Z, _rot.W);
        path = msg.Path;
        this.playerIndex = playerIndex;
    }

    public static CreateObjCmd CreatePlayer(int frame, int playerIndex, GameObject playerPrefab)
    {
        return new CreateObjCmd(frame, playerIndex) { pos = Vector3.zero, rot = Quaternion.identity, prefab = playerPrefab };
    }
    public override void Execute()
    {
        GameObject go;
        if (prefab != null)
            go = GameObject.Instantiate(prefab, pos, rot) as GameObject;
        else
            go = GameObject.Instantiate(Resources.Load(path), pos, rot) as GameObject;
        go.GetUdpNetwork().ownerIndex = playerIndex;
        var behaviours = go.GetComponentsInChildren<FrameBehaviour>(false);
        for (int i = 0; i < behaviours.Length; i++) {
            behaviours[i].Init();
        }
    }
}
