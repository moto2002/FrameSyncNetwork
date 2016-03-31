using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using world_messages;
using System;

public class OutOfRoomContent : MonoBehaviour {
    public InputField capacity;
    public void OnCreateRoomBtn()
    {
        int cap = 0;
        if (int.TryParse(capacity.text, out cap)) {
            if (cap > 0) {
                WorldMessageDispatcher.Instance.Send<MsgCreateRoomReply>(MessageType.CreateRoom, new ArraySegment<byte>(System.BitConverter.GetBytes(cap)), (msg) =>
                {
                    var room = new Room() { id = msg.Id, capacity = msg.Capacity };
                    WorldUIRoot.Instance.AddRoom(room);
                });
            }
        }
    }
}
