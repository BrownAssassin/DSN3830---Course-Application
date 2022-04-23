using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel
{
    Button loginButton;

    public override void OnEnter()
    {

        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(OnLoginClick);
    }
    
    private void OnLoginClick()
    {
        uiMng.PushPanel(UIPanelType.Login);
    }
}
