using UnityEngine;
using System.Collections.Generic;
using FlatBuffers;
using WorldMessages;
using System;
public class WorldUITest : MonoBehaviour
{
    sealed class Room {
        public string id;
        public int playerCount;
    }
    List<Room> rooms = new List<Room>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("GetRoomList"))
        {
            WorldNetwork.Instance.Send<GetRoomListReply>(MessageType.GetRoomList, new ArraySegment<byte>(new byte[0]), (msg) =>
            {
                rooms.Clear();
                for (int i = 0, imax = msg.RoomLength; i < imax; i++)
                {
                    var item = msg.GetRoom(i);
                    rooms.Add(new Room() { id = item.Id, playerCount = item.PlayerCount});
                }
            });
        }
        if (GUILayout.Button("CreateRoom"))
        {
            WorldNetwork.Instance.Send<CreateRoomReply>(MessageType.CreateRoom, new ArraySegment<byte>(new byte[0]), (msg) =>
            {
                rooms.Add(new Room() { id = msg.Id, playerCount = 1});
            });
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("rooms: ");
        GUILayout.BeginScrollView(new Vector2());
        for (int i = 0, imax = rooms.Count; i < imax; i++)
        {
            if (GUILayout.Button(string.Format("Id: {0}, PlayerCount: {1}", rooms[i].id, rooms[i].playerCount))) {
                UserInfo.Instance.RoomId = rooms[i].id;
                Application.LoadLevel(WorldNetwork.Instance.RoomSceneName);
            }
        }
        GUILayout.EndScrollView();
    }
}
