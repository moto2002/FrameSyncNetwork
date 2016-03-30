using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Room
{
    public int id;
    public int playerCount;
    public int capacity;
	public List<int> players = new List<int>();
	public int DynamicPlayerCount{
		get{
			return players.Count;
		}
	}
}