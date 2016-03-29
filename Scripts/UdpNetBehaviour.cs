using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
public class UdpNetBehaviour : MonoBehaviour {
    class RpcMethod
    {
        public MethodInfo methodInfo;
        public object target;
        public UdpRpc attribute;
        public ArraySegment<byte> GetParamBuf(object[] args)
        {
            if (attribute.rpcMsgType == null)
                return new ArraySegment<byte>(DataPaser.ParamObjectsToBytes(args));
            else
                return DataPaser.Instance.SerializeParams(attribute.rpcMsgType, args);
        }

        public void Execute(object[] args)
        {
            methodInfo.Invoke(target, args);
        }
        public void Execute(ArraySegment<byte> seg)
        {
            if(attribute.rpcMsgType == null)
                methodInfo.Invoke(target, DataPaser.BytesToParams(seg.Array, seg.Offset, methodInfo));
            else
            {
                object[] args = DataPaser.Instance.DeserializeToParams(attribute.rpcMsgType, seg);
                methodInfo.Invoke(target, args);
            }

        }
    }
    [HideInInspector]
    public string ownerId;
    Dictionary<string, RpcMethod> rpcTbl = new Dictionary<string, RpcMethod>();
    public int NetId
    {
        get;
        private set;
    }
    
	// Use this for initialization

    protected virtual void Awake()
    { 
        NetId = UdpNetManager.Instance.NextNetId;
        UdpNetManager.Instance.AddBehaviour(this);
    }
    protected virtual void Start()
    {
        var componets = GetComponents<MonoBehaviour>();
        for (int i = 0; i < componets.Length; i++)
        {
            var item = componets[i];
            if (item)
            { 
                var type = item.GetType();
                var attr = System.Attribute.GetCustomAttribute(type, typeof(UseNetBehaviour));
                if (attr != null)
                {
                    AddRpcTable(item);
                }
            }
        }
	}

    public void AddRpcTable(Component comp)
    {
        var methods = comp.GetType().GetMethods();
        for (int i = 0; i < methods.Length; i++) {
            var m = methods[i];
            var attrs = m.GetCustomAttributes(typeof(UdpRpc), false);
            if (attrs.Length > 0)
            { 
                rpcTbl[m.Name] = new RpcMethod() { methodInfo = m, target = comp, attribute = attrs[0] as UdpRpc};
            }
        }
    }

    public void RPC(string methodName, RPCMode mode, params object[] args)
    {
        var argBuf = rpcTbl[methodName].GetParamBuf(args);
        UdpNetManager.Instance.RequestRpc(FrameController.Instance.GetExecuteFrame, this, methodName, argBuf);
        FrameController.Instance.RegisterCurrentCommand();
    }

    public void InvokeRpc(messages.MsgRpc msg)
    {
        var buf = msg.Argbuf.ToByteArray();
        rpcTbl[msg.Method].Execute(new ArraySegment<byte>(buf, 0, buf.Length));
    }
}
