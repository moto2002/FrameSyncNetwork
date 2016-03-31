using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using world_messages;
using System;

public class WorldUIRoot : AutoCreateSingleTon<WorldUIRoot> {
    public GridLayoutGroup grid;
    List<Room> rooms = new List<Room>();
	// Use this for initialization
    void Start()
    {
        if (!WorldMessageDispatcher.Instance.Connectd)
        {
            WorldMessageDispatcher.Instance.RegisterCallBack<MsgPlayerEnterRoom>(MsgPlayerEnterRoom.ParseFrom,
            (msg) =>
            {
                var roomId = msg.RoomId;
                var room = rooms.Find(x => x.id == roomId);
                if (room != null) {
                    var player = msg.PlayerIndex;
                    if (!room.players.Contains(player))
                    {
                        room.PlayerEnterRoom(player);
                    }
                    var myRoom = UserInfo.Instance.Room;
                    if(myRoom != null && myRoom.id == roomId)
                    {
                        room.players.Add(player);
                        if (room.DynamicPlayerCount == room.capacity)
                        {
                            Application.LoadLevel(WorldMessageDispatcher.Instance.RoomSceneName);
                        }
                    }
                }
            }
            );
            WorldMessageDispatcher.Instance.Connect(GetRoomList);
        }
    }

    void GetRoomList() {
        WorldMessageDispatcher.Instance.Send<MsgGetRoomListReply>(MessageType.GetRoomList, new ArraySegment<byte>(new byte[0]), (msg) =>
        {
            rooms.Clear();
            for (int i = 0, imax = msg.RoomsCount; i < imax; i++)
            {
                var item = msg.GetRooms(i);
                var room = new Room() { id = item.Id, capacity = item.Capacity, playercount = item.PlayerCount };
                AddRoom(room);
            }

        });
    }
    
    public void AddRoom(Room room)
    {
        rooms.Add(room);
        var roomItem = DynamicPanelController.Create("UI/RoomItem", grid.transform).GetComponent<UIRoomItem>();
        roomItem.Load(room);
    }
}
