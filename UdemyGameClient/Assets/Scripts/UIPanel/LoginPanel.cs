using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class LoginPanel : BasePanel
{

    private Button closeButton;

    private InputField usernameIF;
    private InputField passwordIF;
    private Button enterButton;
    private Button registerButton;
    private LoginRequest loginRequest;
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnim();
        
    }
    private void Start()
    {

        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        enterButton = transform.Find("EnterButton").GetComponent<Button>();
        registerButton = transform.Find("RegisterButton").GetComponent<Button>();

        enterButton.onClick.AddListener(OnEnterClick);
        registerButton.onClick.AddListener(OnRegisterClick);
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);
        loginRequest = GetComponent<LoginRequest>();
    }

    private void OnRegisterClick()
    {
        uiMng.PushPanel(UIPanelType.Register);
    }
    private void OnEnterClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "please fill the username blank";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "please fill the password blank";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
            return;
        }
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    private void OnCloseClick()
    {
        transform.DOScale(0, 0.4f);
      Tweener tweener=  transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
        tweener.OnComplete(() => uiMng.PopPanel());
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("login succesfull");
            uiMng.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            uiMng.ShowMessageSync("wrong password or username");
        }
    }
   private void EnterAnim()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.3f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);

    }
    private void HideAnim()
    {
        transform.DOScale(0, 0.4f);
        transform.DOLocalMoveX(1000, 0.4f).OnComplete(()=>gameObject.SetActive(false));
    }
    public override void OnResume()
    {
        base.OnResume();
        EnterAnim();
    }
    public override void OnPause()
    {
        base.OnPause();
        transform.DOScale(0, 0.4f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
        tweener.OnComplete(() => gameObject.SetActive(false));
    }
}
