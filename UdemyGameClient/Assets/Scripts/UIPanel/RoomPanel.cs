using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
using System;

public class RoomPanel : BasePanel
{

    private Text localPlayerUsername, localPlayerTotalCount, localPlayerWinCount;
    private Text enemyPlayerUsername, enemyPlayerTotalCount, enemyPlayerWinCount;
    private bool isPopPanel = false;
    private bool udBool = false;
    private bool join1 = false;
    private bool join2 = false;
    private Transform BluePanel, RedPanel;
    private UserData ud = null;
    private UserData ud1 = null;
    private UserData ud2 = null;
    private StartGameRequest sgRequest;
    private void Start()
    {

        localPlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();
        enemyPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();
        BluePanel = transform.Find("BluePanel").GetComponent<Transform>();
        RedPanel = transform.Find("RedPanel").GetComponent<Transform>();
        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartClick);
        sgRequest = GetComponent<StartGameRequest>();
    }


    public void OnExitResponse()
    {
        isPopPanel = true;
    }
    private void Update()
    {
        if (ud != null)
        {

            SetLocalPlayerRes(ud.username, ud.totalCount, ud.winCount);
            ClearEnemyPlayerRes();
            ud = null;
        }
        if (udBool)
        {
            SetLocalPlayerRes(ud.username, ud.totalCount, ud.winCount);
            ClearEnemyPlayerRes();
            udBool = false;
        }
        if (join1)
        {
            SetLocalPlayerRes(ud1.username, ud1.totalCount, ud2.winCount);
            if (join2)
            {
                SetEnemyPlayerRes(ud2.username, ud2.totalCount, ud2.winCount);
            }
            else
                ClearEnemyPlayerRes();
            join1 = false;
            join2 = false;
        }


        if (isPopPanel)
        {
            uiMng.PopPanel();
            isPopPanel = false;
        }

    }

    private void OnStartClick()
    {
        sgRequest.SendRequest();
    }



    public void SetLocalPlayerResSync()
    {
        ud = facade.GetUserData();
        udBool = true;
    }
    public void SetAllPlayerResSync(UserData ud1, UserData ud2)
    {
        this.ud1 = ud1;
        join1 = true;

        this.ud2 = ud2;

        join2 = true;



    }

    public void SetLocalPlayerRes(string username, int totalCount, int winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = totalCount.ToString();
        localPlayerWinCount.text = winCount.ToString();
    }

    public void SetEnemyPlayerRes(string username, int totalCount, int winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = totalCount.ToString();
        enemyPlayerWinCount.text = winCount.ToString();
    }
    public void ClearEnemyPlayerRes()
    {
        enemyPlayerUsername.text = "";
        enemyPlayerTotalCount.text = "Waiting";
        enemyPlayerWinCount.text = "";
    }
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        BluePanel.localPosition = new Vector3(-1000, 0, 0);
        BluePanel.DOLocalMoveX(-200, 0.5f);
        RedPanel.localPosition = new Vector3(1000, 0, 0);
        RedPanel.DOLocalMoveX(300, 0.5f);

    }
    private void ExitAnim()
    {
        BluePanel.DOLocalMoveX(-1000, 0.5f);
        RedPanel.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    public override void OnPause()
    {
        ExitAnim();
    }
    public override void OnResume()
    {

        EnterAnim();
    }
    public override void OnEnter()
    {
        if (BluePanel != null)
            EnterAnim();
    }
    public override void OnExit()
    {
        ExitAnim();
    }

    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.PushPanelSync(UIPanelType.Game);
            facade.EnterPlayingSync();
        }
        else
        {
            uiMng.ShowMessageSync("You're not the host");
        }
    }
}
