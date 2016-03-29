using UnityEngine;
using System.Collections.Generic;
using messages;
public class EmptyCommandEmiter
{
    struct FrameCommand
    {
        public int frame;
        public GenMessage msg;
    }
    int lastCmdFrame = -1;
    public void AddCommand(int frame)
    {
        lastCmdFrame = frame;
    }

    public void Update(int frame) {
        int interval = FrameController.ExecuteFrame;
        if (frame % interval == interval - 1 &&  frame - lastCmdFrame >= interval) {
            AddCommand(frame);
			UdpNetManager.Instance.RequestEmpty(frame + 1 + FrameController.PushBack * interval);
        }
    }
}
