using UnityEngine;
using System.Collections;

public abstract class FrameSingleton<T> : FrameBehaviour where T : FrameSingleton<T>
{
    public static T Instance
    {
        get;
        private set;
    }

    protected override void OnInit()
    {
        base.OnInit();
        Instance = this as T;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Instance = null;
    }
}
