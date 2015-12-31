using UnityEngine;
using System.Collections.Generic;
using FlatBuffers;
using WorldMessages;
using System;
public class WorldUITest : MonoBehaviour
{
    List<Room> rooms = new List<Room>();
    string numStr = "Input Room Capacity";
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
                    rooms.Add(new Room() { id = item.Id, playerCount = item.PlayerCount, capacity = item.Capacity});
                }
            });
        }
        numStr = GUILayout.TextArea(numStr, GUILayout.MinWidth(200));
        if (GUILayout.Button("CreateRoom"))
        {
            int capacity = 0;
            if (int.TryParse(numStr, out capacity))
            {
                WorldNetwork.Instance.Send<CreateRoomReply>(MessageType.CreateRoom, new ArraySegment<byte>(System.BitConverter.GetBytes(capacity)), (msg) =>
                {
                    rooms.Add(new Room() { id = msg.Id, playerCount = 0 , capacity = msg.Capacity});
                });
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("rooms: ");
        GUILayout.BeginScrollView(new Vector2());
        for (int i = 0, imax = rooms.Count; i < imax; i++)
        {
            if (GUILayout.Button(string.Format("Id: {0}, PlayerCount: {1}", rooms[i].id, rooms[i].playerCount))) {
				UserInfo.Instance.Room = rooms[i];
                Application.LoadLevel(WorldNetwork.Instance.RoomSceneName);
            }
        }
        GUILayout.EndScrollView();
    }
}
