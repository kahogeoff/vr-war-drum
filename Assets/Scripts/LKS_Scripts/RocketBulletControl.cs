using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBulletControl : BulletObject
{
    // Use this for initialization
    void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime);
    }
}
