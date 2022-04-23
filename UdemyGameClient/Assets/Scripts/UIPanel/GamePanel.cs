using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class GamePanel : BasePanel {

    private Text timer;
    private int time = -1;
    // Use this for initialization

    private Button succesButton;
    private Button failButton;
    private RoomListPanel roomListPanel;

	void Start () {
        timer = transform.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);
        succesButton = transform.Find("GameOver/SuccesButton").GetComponent<Button>();
        failButton = transform.Find("GameOver/FailButton").GetComponent<Button>();
        succesButton.gameObject.SetActive(false);
        failButton.gameObject.SetActive(false);
        succesButton.onClick.AddListener(OnResultClick);
        failButton.onClick.AddListener(OnResultClick);
    }

    private void ShowTimer(int time)
    {
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.localScale = Vector3.one;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(()=> timer.gameObject.SetActive(false));
    }
	
    public void ShowTimerSync(int time)
    {
        this.time = time;
    }

	// Update is called once per frame
	void Update ()
    {
        if (this.time > -1)
        {
            ShowTimer(time);
            time = -1;
        }
	}

    private void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        uiMng.PopPanel();
        uiMng.PopPanel();
        facade.GameOver();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public void OnGameOverResponse(ReturnCode returnCode)
    {
        switch (returnCode)
        {
            case ReturnCode.Success:
                succesButton.gameObject.SetActive(true);
                break;
            case ReturnCode.Fail:
                failButton.gameObject.SetActive(true);
                break;
        }
    }
}
