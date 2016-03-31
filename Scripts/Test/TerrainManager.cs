using UnityEngine;
using System.Collections;
public class TerrainManager : AutoCreateSingleTon<TerrainManager>
{
    public Transform[] playerSlot;
    public Color[] colors;
    public void SetPos(int index, Transform trans)
    {
        trans.SetParent(playerSlot[index].parent);
        trans.localPosition = playerSlot[index].localPosition;
    }

    public void SetColor(int index, Renderer render)
    {
        render.material.color = colors[index];
    }
}
