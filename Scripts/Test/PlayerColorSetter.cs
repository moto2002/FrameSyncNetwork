using UnityEngine;
using System.Collections;
using Utility;

public class PlayerColorSetter : FrameBehaviour {

    protected override void OnInit()
    {
        var netWork = this.GetUdpNetwork();
        int index = FrameController.Instance.ListIndexOfPlayer(netWork.ownerIndex);
        TerrainManager.Instance.SetPos(index, transform);
        TerrainManager.Instance.SetColor(index, GetComponentInChildren<Renderer>());
        if (netWork.ownerIndex == UserInfo.Instance.Index) { 
            
        }
        Destroy(this);
    }

    public override void FrameUpdate()
    {
        
    }
}
