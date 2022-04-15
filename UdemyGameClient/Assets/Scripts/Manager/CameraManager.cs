using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : BaseManager {
public CameraManager(GameFacade facade) : base(facade) { }

    private Vector3 originalPos;
    private Vector3 originalRot;

    private GameObject cameraGo;
    private Animator cameraAnim;
    private FollowTarget followTarget;

    public override void OnInit()
    {
        cameraGo = Camera.main.gameObject;
        cameraAnim = cameraGo.GetComponent<Animator>();
        followTarget = cameraGo.GetComponent<FollowTarget>();
        
    }

    public void FollowTarget()
    {
        followTarget.target = facade.GetCurrentRoleGameObject().transform;
        cameraAnim.enabled = false;
        originalPos = cameraGo.transform.position;
        originalRot = cameraGo.transform.eulerAngles;
        Quaternion targetQuaternion = Quaternion.LookRotation(followTarget.target.position - cameraGo.transform.position);
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(()=> followTarget.enabled = true) ;
        
        
        
    }

    public void WalkThroughScene()
    {
        followTarget.enabled = false;
        cameraGo.transform.DOMove(originalPos, 1f);
        cameraGo.transform.DORotate(originalRot, 1f).OnComplete(delegate() 
        {
            cameraAnim.enabled = true;
           

        });
    }

   


}
