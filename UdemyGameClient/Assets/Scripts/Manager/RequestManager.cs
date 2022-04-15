using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RequestManager : BaseManager
{

    public RequestManager(GameFacade facade) : base(facade) { }

    private Dictionary<ActionCode, BaseRequest> requestdDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
    {
        requestdDict.Add(actionCode, baseRequest);
    }
    public void RemoveRequest(ActionCode actionCode)
    {
        requestdDict.Remove(actionCode);
    }

    public override void OnInit()
    {

    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        BaseRequest baseRequest = requestdDict.TryGet<ActionCode, BaseRequest>(actionCode);
        baseRequest.OnResponse(data);
    }
}
