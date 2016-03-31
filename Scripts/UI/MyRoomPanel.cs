using UnityEngine;
using System.Collections;

public class MyRoomPanel : AutoCreateSingleTon<MyRoomPanel> {
    public GameObject inRoom;
    public GameObject outRoom;
    public bool InRoom {
        set {
            inRoom.SetActive(value);
            outRoom.SetActive(!value);
        }
    }
}
