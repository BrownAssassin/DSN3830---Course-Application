using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    private ClientManager clientMng;
    private PlayerManager playerMng;
    private RequestManager requestMng;
    private CameraManager cameraMng;
    private UIManager uiMng;
    private AudioManager audioMng;
    private bool isEnterPlaying = false;

    private static GameFacade _instance;
    public static GameFacade Instance { get { return _instance; } }

    //Use this for initialization

   void Start()
    {


       InitManager();
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }
    private void InitManager()
    {
        clientMng = new ClientManager(this);
        playerMng = new PlayerManager(this);
        requestMng = new RequestManager(this);
        cameraMng = new CameraManager(this);
        uiMng = new UIManager(this);
        audioMng = new AudioManager(this);

        audioMng.OnInit();
        playerMng.OnInit();
        requestMng.OnInit();
        cameraMng.OnInit();
        uiMng.OnInit();
        clientMng.OnInit();
    }
    private void OnDestroy()
    {
        DestroyManager();
    }
    private void DestroyManager()
    {
        audioMng.OnDestroy();
        playerMng.OnDestroy();
        requestMng.OnDestroy();
        cameraMng.OnDestroy();
        uiMng.OnDestroy();
        clientMng.OnDestroy();
    }

    //Update is called once per frame
    void Update()
    {
        UpdateManager();
        if (isEnterPlaying)
        {
            EnterPlaying();
            isEnterPlaying = false;
        }
    }

    private void UpdateManager()
    {
        audioMng.Update();
        playerMng.Update();
        requestMng.Update();
        cameraMng.Update();
        uiMng.Update();
        clientMng.Update();
    }

    public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
    {
        requestMng.AddRequest(actionCode, baseRequest);
    }
    public void RemoveRequest(ActionCode actionCode)
    {
        requestMng.RemoveRequest(actionCode);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestMng.HandleResponse(actionCode, data);
    }
    public void ShowMessage(string msg)
    {
        uiMng.ShowMessage(msg);
    }
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientMng.SendRequest(requestCode, actionCode, data);
    }
    public void SetUserData(UserData ud)
    {
        playerMng.UserData = ud;
    }
    public UserData GetUserData()
    {
        return playerMng.UserData;
    }
    public void SetCurrentRoleType(RoleType rt)
    {
        playerMng.SetCurrentRoleType(rt);
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return playerMng.GetCurrentRoleGameObject();
    }
    public void EnterPlayingSync()
    {
        isEnterPlaying = true;
    }
    private void EnterPlaying()
    {
        playerMng.SpawnRoles();
        cameraMng.FollowTarget();
    }
    public void StartPlaying()
    {
        playerMng.AddControlScript();
        playerMng.CreateSyncRequest();
    }

    public void SendAttack(int damage)
    {
        playerMng.SendAttack(damage);
    }

    public void GameOver()
    {
        playerMng.GameOver();
        cameraMng.WalkThroughScene();
    }

}
