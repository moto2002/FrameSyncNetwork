using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UserInfo
{
    UserData data;
    static UserData mInstance;
    public static UserData Instance{
        get {
            if (mInstance == null) {
                mInstance = new UserData();
            }
            return mInstance;
        }
    }
    private UserInfo() { 
        
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
}
