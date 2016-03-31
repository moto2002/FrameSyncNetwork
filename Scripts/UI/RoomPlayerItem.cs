using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomPlayerItem : MonoBehaviour {
    public Text index;
    public void Load(int playerIndex)
    {
        index.text = playerIndex.ToString();
    }
}
