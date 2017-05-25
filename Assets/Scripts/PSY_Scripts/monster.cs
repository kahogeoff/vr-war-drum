using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour {

	public float speed;
	public Transform target;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 3f);
		target = GameObject.FindGameObjectWithTag ("MainCamera").transform;

		transform.position = new Vector3(-4,1,5);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 relativePos = target.position - transform.position;
		//Quaternion rotation = Quaternion.LookRotation(relativePos);
		//transform.rotation = rotation;

		transform.Translate (-1 * Vector3.forward * speed * Time.deltaTime);
	}
}
