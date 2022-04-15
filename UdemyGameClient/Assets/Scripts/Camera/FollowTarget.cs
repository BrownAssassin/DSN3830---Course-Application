using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    private Vector3 offset = new Vector3(0, 6.481546f, -11.07821f);
    private float smoothing = 2f;
	// Use this for initialization
	
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 targetPosition = target.position + offset;
     transform.position=  Vector3.Lerp(transform.position, targetPosition, smoothing*Time.deltaTime);
        transform.LookAt(target);
	}
}
