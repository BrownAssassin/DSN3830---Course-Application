using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    private int id;
    public Text username;
    public Text totalCount;
    public Text winCount;
    public Button joinButton;
    private RoomListPanel panel;

    // Use this for initialization
    void Start()
    {

        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinClick);
        }

    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void OnJoinClick()
    {
        panel.OnJoinClick(id);
    }


    public void SetRoomInfo(int id, string username, string totalCount, string winCount, RoomListPanel panel)
    {
        this.id = id;
        this.username.text = username;
        this.totalCount.text = totalCount;
        this.winCount.text = winCount;
        this.panel = panel;
    }
    // Update is called once per frame
    void Update()
    {

    }
}

