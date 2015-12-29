using UnityEngine;
using System.Collections.Generic;
using FlatBuffers;
using WorldMessages;
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
        if (GUILayout.Button("GetRoomList"))
        {
            WorldNetwork.Instance.Send<GetRoomListReply>(MessageType.GetRoomList, new ByteBuffer(new byte[0], 0), (msg) =>
            {
                rooms.Clear();
                for (int i = 0, imax = msg.RoomLength; i < imax; i++)
                {
                    var item = msg.GetRoom(i);
                    rooms.Add(new Room() { id = item.Id, playerCount = item.PlayerCount});
                }
            });
        }
        GUILayout.Label("rooms: ");
        for (int i = 0, imax = rooms.Count; i < imax; i++)
        {
            if (GUILayout.Button(string.Format("Id: {0}, PlayerCount: {1}", rooms[i].id, rooms[i].playerCount))) { 
                
            }
        }
    }
}
