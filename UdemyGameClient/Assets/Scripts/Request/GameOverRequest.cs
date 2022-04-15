using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GameOverRequest : BaseRequest {
    private GamePanel panel;
    private bool isGameOver = false;
    ReturnCode returncode;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameOver;
        panel = GetComponent<GamePanel>();
        base.Awake();
    }
    public override void OnResponse(string data)
    {
        returncode = (ReturnCode)int.Parse(data);
        isGameOver = true;
       
    }
    private void Update()
    {
        if (isGameOver)
        {
            panel.OnGameOverResponse(returncode);
            isGameOver = false;
        }
    }

}
