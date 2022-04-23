using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFacade facade) : base(facade) { }
    private UserData ud;
    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();
    private Transform rolePositions;
    private RoleType currentRoleType;
    private GameObject currentRoleGameObject;
    private GameObject remoteRoleGameObject;
    private GameObject playerSyncRequest;
    private ShootRequest shootRequest;
    private AttackRequest attackRequest;

    public UserData UserData
    {
        set { ud = value; }
        get { return ud; }
    }

    public override void OnInit()
    {
        rolePositions = GameObject.Find("RolePositions").transform;
        InitRoleDataDict();
    }

    private void InitRoleDataDict()
    {
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "BluePlayer", "BlueArrow", rolePositions.Find("Position1").position));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red, "RedPlayer", "RedArrow", rolePositions.Find("Position2").position));
    }

    public void SpawnRoles()
    {
        foreach(RoleData rd in roleDataDict.Values)
        {
            GameObject go=Instantiate(rd.rolePrefab, rd.spawnPosition, Quaternion.identity);
            if (currentRoleType == rd.roleType)
            {
                currentRoleGameObject = go;
                currentRoleGameObject.GetComponent<PlayerInfo>().isLocal = true;
            }
            else
            {
                remoteRoleGameObject = go;
            }
        }
    }

    public void SetCurrentRoleType(RoleType rt)
    {
        this.currentRoleType = rt;
    }

    public GameObject GetCurrentRoleGameObject()
    {
        return currentRoleGameObject;
    }

    public void AddControlScript()
    {
        currentRoleGameObject.AddComponent<PlayerMove>();
       PlayerAttack playerAttack= currentRoleGameObject.AddComponent<PlayerAttack>();
        RoleType rt = currentRoleGameObject.GetComponent<PlayerInfo>().roleType;
        RoleData rd = GetRoleDataByRoleType(rt);
        playerAttack.arrowPrefab = rd.arrowPrefab;
        playerAttack.SetPlayerManager(this);
    }

    private RoleData GetRoleDataByRoleType(RoleType rt)
    {
        RoleData rd = null;
        roleDataDict.TryGetValue(rt, out rd);
        return rd;
    }

    public void CreateSyncRequest()
    {
        playerSyncRequest= new GameObject("PlayerSyncRequest");
        playerSyncRequest.AddComponent<MoveRequest>().SetLocalPlayer(currentRoleGameObject.transform,currentRoleGameObject.GetComponent<PlayerMove>()).SetRemotePlayer(remoteRoleGameObject.transform);
        shootRequest= playerSyncRequest.AddComponent<ShootRequest>();
        shootRequest.playerMng = this;
        attackRequest= playerSyncRequest.AddComponent<AttackRequest>();
    }

    public void Shoot(GameObject arrowPrefab,Vector3 pos,Quaternion rotation)
    {
        Instantiate(arrowPrefab, pos, rotation).GetComponent<Arrow>().isLocal = true;
        shootRequest.SendRequest(arrowPrefab.GetComponent<Arrow>().roleType, pos, rotation.eulerAngles);
        //remoteRoleGameObject.GetComponent<Animator>().SetTrigger("Attack");
    }

   public void RemoteShoot(RoleType roleType,Vector3 pos,Vector3 rot)
    {
        GameObject arrowPrefab = GetRoleDataByRoleType(roleType).arrowPrefab;
        Transform ts=Instantiate(arrowPrefab).GetComponent<Transform>();
        ts.position = pos;
        ts.eulerAngles = rot;
    } 

    public void SendAttack(int damage)
    {
        attackRequest.SendRequest(damage);
    }

    public void GameOver()
    {
        Destroy(currentRoleGameObject);
        Destroy(remoteRoleGameObject);
        Destroy(playerSyncRequest);
        attackRequest = null;
        shootRequest = null;
    }
}

