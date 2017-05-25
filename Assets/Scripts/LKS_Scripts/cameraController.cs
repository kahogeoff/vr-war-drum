using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    public float speed = 5f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(-transform.forward * speed *Time.deltaTime* Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(-transform.right * speed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }
    }
}
