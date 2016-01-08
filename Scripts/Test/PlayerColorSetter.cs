using UnityEngine;
using System.Collections;
using Utility;

public class PlayerColorSetter : FrameBehaviour {

    protected override void OnInit()
    {
        var netWork = this.GetUdpNetwork();
        int index = FrameController.Instance.IndexOfPlayer(netWork.ownerId);
        TerrainManager.Instance.SetPos(index, transform);
        TerrainManager.Instance.SetColor(index, GetComponentInChildren<Renderer>());
        if (netWork.ownerId == UserInfo.Instance.Id) { 
            
        }
        Destroy(this);
    }

    public override void FrameUpdate()
    {
        
    }
}
