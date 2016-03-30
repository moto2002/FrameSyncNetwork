using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using world_messages;
public class WorldUITest : MonoBehaviour
{
    List<Room> rooms = new List<Room>();
    string numStr = "Input Room Capacity";
    // Use this for initialization
    void Start()
    {
        WorldNetwork.Instance.RegisterCallBack<MsgPlayerEnterRoom>(MsgPlayerEnterRoom.ParseFrom, 
		(msg) =>
		{
			var room = UserInfo.Instance.Room;
			if(room != null && room.id == msg.RoomId){
				var player = msg.PlayerIndex;
				if(!room.players.Contains(player))
				{
					room.players.Add(player);
					if(room.DynamicPlayerCount == room.capacity){
						Application.LoadLevel(WorldNetwork.Instance.RoomSceneName);
					}
				}
			}
		}
		);
        WorldNetwork.Instance.Connect();
	}

    // Update is called once per frame
    void OnGUI()
    {
        if (!WorldNetwork.Instance.Connectd)
            return;
        if (UserInfo.Instance.Room == null)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetRoomList"))
            {
                WorldNetwork.Instance.Send<MsgGetRoomListReply>(MessageType.GetRoomList, new ArraySegment<byte>(new byte[0]), (msg) =>
                {
                    rooms.Clear();
                    for (int i = 0, imax = msg.RoomsCount; i < imax; i++)
                    {
                        var item = msg.GetRooms(i);
                        rooms.Add(new Room() { id = item.Id, playerCount = item.PlayerCount, capacity = item.Capacity });
                    }
                });
            }
            numStr = GUILayout.TextArea(numStr, GUILayout.MinWidth(200));
            if (GUILayout.Button("CreateRoom"))
            {
                int capacity = 0;
                if (int.TryParse(numStr, out capacity))
                {
                    WorldNetwork.Instance.Send<MsgCreateRoomReply>(MessageType.CreateRoom, new ArraySegment<byte>(System.BitConverter.GetBytes(capacity)), (msg) =>
                    {
                        rooms.Add(new Room() { id = msg.Id, playerCount = 0, capacity = msg.Capacity });
                    });
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("rooms: ");
            GUILayout.BeginScrollView(new Vector2());
            for (int i = 0, imax = rooms.Count; i < imax; i++)
            {
                var room = rooms[i];
                if (GUILayout.Button(string.Format("Id: {0}, PlayerCount: {1}/{2}", room.id, room.playerCount, room.capacity)))
                {
                    WorldNetwork.Instance.Send<MsgEnterRoomReply>(MessageType.EnterRoom, new ArraySegment<byte>(BitConverter.GetBytes(room.id)), (msg) =>
                    {
                        var result = msg.Result;
                        if (result == EnterRoomResult.Ok)
                        {
                            UserInfo.Instance.Index = msg.AllockedIndex;
                            UserInfo.Instance.Room = room;
                            for (int j = 0, jmax = msg.PlayersCount; j < jmax; j++)
                            {
                                var player = msg.GetPlayers(j);
                                room.players.Add(player);
                            }
                            if (room.DynamicPlayerCount == room.capacity)
                            {
                                Application.LoadLevel(WorldNetwork.Instance.RoomSceneName);
                            }
                        }
                    });
                }
            }
            GUILayout.EndScrollView();
        }
        else { 
            foreach(var item in UserInfo.Instance.Room.players)
            {
                GUILayout.Button(item.ToString());
            }
        }
    }
}
