using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RegisterPanel : BasePanel{

    private InputField usernameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;

    private RegisterRequest registerRequest;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(true);
        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("RePasswordLabel/RePasswordInput").GetComponent<InputField>();

        registerRequest = GetComponent<RegisterRequest>();

        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
    }
    private  void OnCloseClick()
    {
        transform.DOScale(0, 0.4f);
        transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
    }
	private void OnRegisterClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "Do not leave username empty ";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "Do not leave password empty ";
        }
        if (rePasswordIF.text != passwordIF.text)
        {
            msg += "the passwords are note same!!";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
            return;
        }
        registerRequest.SendRequest(usernameIF.text,passwordIF.text);


    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("the registration is succesfull");
        }
        else
        {
            uiMng.ShowMessageSync("the registration is failed");
        }
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.3f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.5f);
    }
    public override void OnExit()
    {
        gameObject.SetActive(false);
    }
}
