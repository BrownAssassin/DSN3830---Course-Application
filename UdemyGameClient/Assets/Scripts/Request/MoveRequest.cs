using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class MoveRequest : BaseRequest {
    private bool isSyncRemotePlayer = false;
    public Transform localPlayerTransform;
    private Transform remoteRolePlayerTransform;
    private Animator remoteRoleAnim;
    public PlayerMove playerMove;
    private int SyncRate = 50;
    Vector3 pos;
    Vector3 rot;
    float forward;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }
    private void Update()
    {
        if (isSyncRemotePlayer)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;

        }
    }
    public MoveRequest SetLocalPlayer(Transform localPlayerTransform,PlayerMove playerMove)
    {
        this.localPlayerTransform = localPlayerTransform;
        this.playerMove = playerMove;
        return this;
    }
    public MoveRequest SetRemotePlayer(Transform remoteRolePlayerTransform)
    {
        this.remoteRolePlayerTransform = remoteRolePlayerTransform;
        this.remoteRoleAnim = remoteRolePlayerTransform.GetComponent<Animator>();
        return this;
    }
    private void Start()
    {
        InvokeRepeating("SyncLocalPlayer", 1f, 1f/SyncRate);
    }

    public void SyncLocalPlayer()
    {
        SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z, localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z, playerMove.forward);
    }
    public void SyncRemotePlayer()
    {
        remoteRolePlayerTransform.position = pos;
        remoteRolePlayerTransform.eulerAngles = rot;
        remoteRoleAnim.SetFloat("Forward", forward);
    }

    private void SendRequest(float xPos,float yPos,float zPos,float xRot,float yRot,float zRot,float forward)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", xPos, yPos, zPos, xRot, yRot, zRot, forward);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos = UnityTools.Parse(strs[0]);
        rot = UnityTools.Parse(strs[1]);
        forward = float.Parse(strs[2]);
        isSyncRemotePlayer = true;
    }

   

}
