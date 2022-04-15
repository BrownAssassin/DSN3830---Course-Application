using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ShootRequest : BaseRequest {
    RoleType rt;
    Vector3 position;
    Vector3 rotation;
    public PlayerManager playerMng;
    private bool isShoot = false;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }
    private void Update()
    {
        if (isShoot)
        {
            playerMng.RemoteShoot(rt, position, rotation);
            isShoot = false;
        }
    }
    public void SendRequest(RoleType rt,Vector3 pos,Vector3 rot)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)rt, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z);

        base.SendRequest(data);
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        rt = (RoleType)int.Parse(strs[0]);
        position = UnityTools.Parse(strs[1]);
        rotation = UnityTools.Parse(strs[2]);
        isShoot = true;
    }
}
