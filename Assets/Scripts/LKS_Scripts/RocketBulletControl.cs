using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBulletControl : MonoBehaviour {
    public float MoveSpeed = 50.0f;
    // Use this for initialization
    void Start () {
        Destroy(this.gameObject, 5.0f);
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime);
    }
}
