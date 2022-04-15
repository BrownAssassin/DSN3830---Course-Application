using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    private Rigidbody rgd;
    public RoleType roleType;
    public bool isLocal = false;
	// Use this for initialization
	void Start () {
        rgd = GetComponent<Rigidbody>();
        transform.position += new Vector3(0, 1, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rgd.MovePosition(transform.position + transform.forward * 5 * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (isLocal)
        {
            bool isLocalPlayer = other.GetComponent<PlayerInfo>().isLocal;
            if (isLocal != isLocalPlayer)
            {
                GameFacade.Instance.SendAttack(50);
            }
        }
        Destroy(this.gameObject);
    }

}
