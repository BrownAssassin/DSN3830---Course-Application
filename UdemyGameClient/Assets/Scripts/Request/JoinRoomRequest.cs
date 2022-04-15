using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class JoinRoomRequest : BaseRequest
{

    RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }
    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }
    public override void OnResponse(string data)
    {
        //returncode,roletype-id,username,totalcount,wincount|id,username,totalcount,wincount
        string[] strs = data.Split('-');
        string[] strs2 = strs[0].Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs2[0]);
        RoleType roleType = (RoleType)int.Parse(strs2[1]);
        facade.SetCurrentRoleType(roleType);
        if (returnCode == ReturnCode.Success)
        {
            string[] udStrArray = strs[1].Split('|');
            UserData ud1 = new UserData(udStrArray[0]);
            UserData ud2 = new UserData(udStrArray[1]);
            roomListPanel.OnJoinResponse(returnCode, ud1, ud2);
        }

    }
}

