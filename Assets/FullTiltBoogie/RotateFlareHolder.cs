using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFlareHolder : MonoBehaviour {
	// THIS SCRIPT SIMPLY ROTATES THE GAME OBJECT HOLDING THE LENS FLARE SCRIPT SO THAT
	// THE USER CAN GET AN IDEA OF HOW THE FLARE LOOKS WHEN IN MOTION
	
	
	void Update () {
		transform.Rotate(Vector3.down * Time.deltaTime * 60f);
		transform.Rotate(Vector3.left * Time.deltaTime * 30f);
		transform.Rotate(Vector3.back * Time.deltaTime * 12f);
	}
}
