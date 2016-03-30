using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UserInfo
{
    UserData data;
    static UserInfo mInstance;
    public static UserInfo Instance
    {
        get {
            if (mInstance == null) {
                mInstance = new UserInfo();
            }
            return mInstance;
        }
    }
    private UserInfo() {
        data = new UserData();
    }

    public void Init(UserData data)
    {
        this.data = data;
    }

    public string Id {
        get {
            return data.id;
        }
    }

    public int Index;

    public Room Room
    {
        get;
        set;
    }
}
