using UnityEngine;
using System.Collections;
public class TerrainManager : AutoCreateSingleTon<TerrainManager>
{
    public Transform[] playerSlot;
    public Color[] colors;
    public void SetPos(int index, Transform trans)
    {
        trans.parent = playerSlot[index];
        trans.localPosition = Vector3.zero;
    }

    public void SetColor(int index, Renderer render)
    {
        render.material.color = colors[index];
    }
}
