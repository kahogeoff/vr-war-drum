using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumStickControl : MonoBehaviour {
    static public Vector2 HITPATTEN_LEFT_RED {
        get { return new Vector2(1,1);  }
    }

    static public Vector2 HITPATTEN_LEFT_BLUE
    {
        get { return new Vector2(1, 2); }
    }

    static public Vector2 HITPATTEN_RIGHT_RED
    {
        get { return new Vector2(2, 1); }
    }

    static public Vector2 HITPATTEN_RIGHT_BLUE
    {
        get { return new Vector2(2, 2); }
    }

    public DrumStick LeftStick;
    public DrumStick RightStick;
    // Use this for initialization
    void Awake() {
        LeftStick.gameObject.SetActive(false);
        RightStick.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        /*
        if(GetBothDrumHitted() == DrumScript.DrumType.Red)
        {
            Debug.Log("Big Red");

        }else if(GetBothDrumHitted() == DrumScript.DrumType.Blue)
        {
            Debug.Log("Big Blue");
        }
        */
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

    /* Below this line is some nothing working well code */
    public Vector2 GetLeftStickHitStatus()
    {
        return new Vector2((int)LeftStick.HittedDrumSide, (int)LeftStick.HittedDrumType);
    }

    public Vector2 GetRightStickHitStatus()
    {
        return new Vector2((int)RightStick.HittedDrumSide, (int)RightStick.HittedDrumType);
    }
    /* Above this line is some nothing working well code */

    public DrumScript.DrumType GetBothDrumHitted()
    {
        if(RightStick.HittedDrumType == LeftStick.HittedDrumType)
        {
            return RightStick.HittedDrumType;
        }
        return DrumScript.DrumType.NotADrum;
    }
}
