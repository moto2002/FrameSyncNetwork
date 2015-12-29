using UnityEngine;
using System.Collections;

public class UserData
{
    public string id;

    public UserData(){
        id = System.Guid.NewGuid().ToString();
    }
}
