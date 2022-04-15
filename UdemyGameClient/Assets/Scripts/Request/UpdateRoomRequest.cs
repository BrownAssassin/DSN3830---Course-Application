using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRoomRequest : BaseRequest
{
    RoomPanel panel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        panel = GetComponent<RoomPanel>();
        base.Awake();
    }
    public override void OnResponse(string data)
    {
        //Ud1
        UserData ud1=null, ud2 = null;
        string[] udStrArray = data.Split('|');
        ud1 = new UserData(udStrArray[0]);

        if (udStrArray.Length > 1)
            ud2 = new UserData(udStrArray[1]);
        panel.SetAllPlayerResSync(ud1, ud2);
    }
}
