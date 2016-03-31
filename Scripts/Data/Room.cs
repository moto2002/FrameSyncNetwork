using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Room : PanelAttachData<Room>
{
    public int id;
    public int capacity;
	public List<int> players = new List<int>();
    public int playercount;
    public int DynamicPlayerCount
    {
		get{
			return players.Count;
		}
	}

    public void PlayerEnterRoom(int playerIndex)
    {
        playercount++;
        FreshPanel();
    }
}