using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using world_messages;

public class UIRoomItem : DataFollowPanel<Room>
{
    public Text count_capacity;
    public Text roomId;
    public Image memberProcess;

    public void OnEnterRoomBtn()
    {
        if (UserInfo.Instance.Room != null)
            return;
        if (data.playercount >= data.capacity)
            return;
        WorldMessageDispatcher.Instance.Send<MsgEnterRoomReply>(MessageType.EnterRoom, new ArraySegment<byte>(BitConverter.GetBytes(data.id)), (msg) =>
        {
            var result = msg.Result;
            if (result == EnterRoomResult.Ok)
            {
                UserInfo.Instance.Index = msg.AllockedIndex;
                data.PlayerEnterRoom(UserInfo.Instance.Index);
                UserInfo.Instance.Room = data;
                for (int j = 0, jmax = msg.PlayersCount; j < jmax; j++)
                {
                    var player = msg.GetPlayers(j);
                    data.players.Add(player);
                }
                MyRoomPanel.Instance.InRoom = true;
                MyRoomPanel.Instance.inRoom.GetComponent<InRoomContent>().Load(data);
                if (data.DynamicPlayerCount == data.capacity)
                {
                    Application.LoadLevel(WorldMessageDispatcher.Instance.RoomSceneName);
                }
            }
        });
    }

    protected override void ShowContent(Room data)
    {
        count_capacity.text = data.playercount + "/" + data.capacity;
        roomId.text = "NO." + data.id.ToString();
        memberProcess.fillAmount = (float)data.playercount / data.capacity;
    }
}
