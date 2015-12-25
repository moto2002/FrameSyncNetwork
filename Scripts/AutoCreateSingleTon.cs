using UnityEngine;
using System.Collections;

public class AutoCreateSingleTon<T> : MonoBehaviour where T : AutoCreateSingleTon<T>
{
    private static T mInstance;
    public static T Instance {
        get {
            if (mInstance == null)
            {
                new GameObject(typeof(T).Name, typeof(T));
            }
            return mInstance;
        }
    }

    protected virtual void Awake()
    {
        mInstance = this as T;
    }
	// Use this for initialization
    protected virtual void OnDestroy()
    {
        mInstance = null;
    }
	
	// Update is called once per framess
}
