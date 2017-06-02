using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour {

	public float speed = 100.0f;
	public Transform target;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 3f);
		target = GameObject.FindGameObjectWithTag ("Hitpoints").transform;

		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
	}
}
