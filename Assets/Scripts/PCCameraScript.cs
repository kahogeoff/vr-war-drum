using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCameraScript : MonoBehaviour {
    //public float shakeAmount = 1.0f;
    //public float shakeDuration = .01f;

    [SerializeField]
    DrumStickControl drumStickControl;
        
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(LevelStateManager.GlobalCurrentState == LevelStateManager.LevelState.InGame) {
		    if(drumStickControl.GetLeftStickHitStatus()!=Vector2.zero || drumStickControl.GetRightStickHitStatus() != Vector2.zero)
            {
                //GetComponent<CameraShake>().ShakeCamera(shakeAmount, shakeDuration);
            }
        }
    }
}
