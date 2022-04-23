using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class RoomListPanel : BasePanel
{
    private UserData ud1 = null;
    private UserData ud2 = null;
    private bool join1 = false;
    private bool join2 = false;
    private RectTransform battleRes;
    private RectTransform roomList;

    private ListRoomRequest lrRequest;
    private GameObject roomItemPrefab;
    private VerticalLayoutGroup roomLayout;
    private List<UserData> udList = null;
    private CreateRoomRequest crRequest;
    private JoinRoomRequest jrRequest;

    private void Start()
    {

        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomClick);
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        transform.Find("RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshClick);
        lrRequest = GetComponent<ListRoomRequest>();
        crRequest = GetComponent<CreateRoomRequest>();
        jrRequest = GetComponent<JoinRoomRequest>();
    }

    public override void OnEnter()
    {
        if (battleRes != null)
        {
            EnterAnim();
        }

        SetBattleRes();

        if (lrRequest == null)
        {
            lrRequest = GetComponent<ListRoomRequest>();
        }

        lrRequest.SendRequest();
    }

    public override void OnResume()
    {
        EnterAnim();
        lrRequest.SendRequest();
    }

    public override void OnPause()
    {
        HideAnim();
    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);
        battleRes.transform.localPosition = new Vector3(-1000, 0, 0);
        battleRes.DOLocalMoveX(-350, 0.5f);

        roomList.transform.localPosition = new Vector3(1000, 0, 0);
        roomList.DOLocalMoveX(145, 0.5f);
    }

    private void HideAnim()
    {
        battleRes.DOLocalMoveX(-1000, 0.5f);
        Tweener tweener = roomList.DOLocalMoveX(1000, 0.5f);
        tweener.OnComplete(() => gameObject.SetActive(false));
    }

    private void OnCloseClick()
    {
        uiMng.PopPanel();
    }

    public override void OnExit()
    {
        HideAnim();
    }

    private void SetBattleRes()
    {
        UserData ud = facade.GetUserData();
        transform.Find("BattleRes/Username").GetComponent<Text>().text = ud.username;
        transform.Find("BattleRes/TotalCount").GetComponent<Text>().text = ud.totalCount.ToString();
        transform.Find("BattleRes/WinCount").GetComponent<Text>().text = ud.winCount.ToString();
    }

    public void LoadRoomItemSync(List<UserData> udList)
    {
        this.udList = udList;
    }

    private void LoadRoomItem(List<UserData> udList)
    {
        RoomItem[] riArray = roomLayout.GetComponentsInChildren<RoomItem>();

        foreach (RoomItem ri in riArray)
        {
            ri.DestroySelf();
        }

        int count = udList.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
            UserData ud = udList[i];
            roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.id, ud.username, ud.totalCount.ToString(), ud.winCount.ToString(), this);

        }

        int roomCount = GetComponentsInChildren<RoomItem>().Length;
    }

    public void OnJoinClick(int id)
    {
        jrRequest.SendRequest(id);
    }

    private void OnCreateRoomClick()
    {
        BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
        crRequest.SendRequest();
        crRequest.SetPanel(panel);
    }

    private void OnRefreshClick()
    {
        lrRequest.SendRequest();
    }

    private void Update()
    {
        if (udList != null)
        {
            LoadRoomItem(udList);
            udList = null;
        }

        if (join1)
        {
            BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResSync(ud1, ud2);
            join1 = false;
        }
    }

    public void OnJoinResponse(ReturnCode returncode, UserData ud1, UserData ud2)
    {
        switch (returncode)
        {
            case ReturnCode.NotFound:
                uiMng.ShowMessageSync("room not found");
                break;
            case ReturnCode.Fail:
                uiMng.ShowMessageSync("failed to join");
                break;
            case ReturnCode.Success:
                this.ud1 = ud1;
                this.ud2 = ud2;
                join1 = true;
                break;
        }
    }
}
