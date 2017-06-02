using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {
    public bool IsPaused = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            if (!IsPaused)
            {
                Time.timeScale = 0;
                IsPaused = true;
            }
            else
            {
                Time.timeScale = 1.0f;
                IsPaused = false;
            }
        }
	}
}
