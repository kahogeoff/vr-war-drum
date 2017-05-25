﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightRocketControll : MonoBehaviour
{
    public Animator anim;
    public bool fired = false;
    public GameObject Bullet;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            Instantiate(Bullet, new Vector3(-4.7f, 1.5f, -8.5f), Quaternion.identity);
            anim.Play("RightRocketAnimation", -1, 0f);
            fired = false;
        }
        /*****************/
        /* test hit drum */
        /*****************/
        if (Input.GetKeyDown(KeyCode.Y))
        {

            fired = true;

        }
    }
}
