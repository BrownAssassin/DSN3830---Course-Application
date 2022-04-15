using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour {

    protected UIManager uiMng;
    protected GameFacade facade;
    public GameFacade _facade
    {
        set { facade = value; }
    }
    
    public UIManager UIMng
    {
        set { uiMng = value; }
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnPause()
    {

    }

    public virtual void OnResume()
    {

    }

    public virtual void OnExit()
    {

    }
}
