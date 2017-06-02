using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumStickControl : MonoBehaviour {
    public DrumStick LeftStick;
    public DrumStick RightStick;
    // Use this for initialization
    void Awake() {
        LeftStick.gameObject.SetActive(false);
        RightStick.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("LeftTrigger") > .9f) {
            LeftStick.gameObject.SetActive(true);
            LeftStick.gameObject.SendMessage("Start");
        }
		if(Input.GetAxis("RightTrigger") > .9f) {
            RightStick.gameObject.SetActive(true);
            RightStick.gameObject.SendMessage("Start");
        }
    }
}
