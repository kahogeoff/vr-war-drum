using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftGunControll : MonoBehaviour {
    public Animator anim;
    public bool fired = false;
    public GameObject explision;
    public GameObject Bullet;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (fired)
        {
            Instantiate(explision, new Vector3(3.6f, -1.0f, -7f), Quaternion.identity);
            Instantiate(Bullet, new Vector3(3.6f, -1.0f, -7f), Quaternion.identity);
            anim.Play("LeftGunAnimation", -1, 0f);
            fired = false;
        }
        /*****************/
        /* test hit drum */
        /*****************/
        if (Input.GetKeyDown(KeyCode.R))
        {
            fired = true;
        }
    }
}
