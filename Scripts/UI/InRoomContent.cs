using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InRoomContent : DataFollowPanel<Room> {
    public Text count_capacity;
    public GridLayoutGroup grid;
    protected override void ShowContent(Room data)
    {
        grid.GridLoadChild<RoomPlayerItem, int>(data.players, (d) => "UI/PlayerItem", (c, d, i) => c.Load(d), null);
    }
}
