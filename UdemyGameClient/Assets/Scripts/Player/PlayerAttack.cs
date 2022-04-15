using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Animator anim;
    public GameObject arrowPrefab;
    private Transform leftHandTrans;
    private PlayerManager playerMng;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        leftHandTrans = transform.Find("Erika_Archer_Meshes/Erika_Archer_Bow_Mesh");
        leftHandTrans.position += new Vector3(0, 0, 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
               bool isCollider= Physics.Raycast(ray, out hit);
                if (isCollider)
                {
                    Vector3 targetPoint = hit.point;
                    anim.SetTrigger("Attack");
                    targetPoint.y = transform.position.y;
                    Vector3 dir = targetPoint - transform.position;
                    transform.rotation = Quaternion.LookRotation(dir);
                    Shoot(dir);
                }
            }
        }
	}
    public void SetPlayerManager(PlayerManager playerMng)
    {
        this.playerMng = playerMng;
    }
    private void Shoot(Vector3 dir)
    {
        playerMng.Shoot(arrowPrefab, leftHandTrans.position, Quaternion.LookRotation(dir));
    
    }

}
