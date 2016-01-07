using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Room
{
    public string id;
    public int playerCount;
    public int capacity;
	public List<string> players = new List<string>();
	public int DynamicPlayerCount{
		get{
			return players.Count;
		}
	}
}