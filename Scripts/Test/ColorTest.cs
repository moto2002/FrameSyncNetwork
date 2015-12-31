using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
[ExecuteInEditMode]
public class ColorTest : MonoBehaviour {
    void OnEnable()
    {
        float[] fa = new float[] { 0, 1, 0, 1};
        unsafe
        {
            fixed (void* fp = fa)
            {
                Color* cp = (Color*)fp;
                Color c = cp[0];
                Debug.Log(c);
            }
        }
    }
}
