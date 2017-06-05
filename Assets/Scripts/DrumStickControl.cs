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
        
        if(GetBothDrumHitted() == DrumScript.DrumType.Red)
        {
            Debug.Log("Big Red");

        }else if(GetBothDrumHitted() == DrumScript.DrumType.Blue)
        {
            Debug.Log("Big Blue");
        }
    }

    void LeftStickOn()
    {
        LeftStick.gameObject.SetActive(true);
        LeftStick.gameObject.SendMessage("Start");
    }

    void RightStickOn()
    {
        RightStick.gameObject.SetActive(true);
        RightStick.gameObject.SendMessage("Start");
    }

    public DrumScript.DrumType GetBothDrumHitted()
    {
        if(RightStick.HittedDrumType == LeftStick.HittedDrumType)
        {
            return RightStick.HittedDrumType;
        }
        return DrumScript.DrumType.NotADrum;
    }
}
