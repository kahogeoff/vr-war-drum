using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateScore : MonoBehaviour {

    static public int score = 0;
    static public int combo = 0;
    static public int maxcombo = 0;

    private Transform[] perfecthitpositions;

	// Use this for initialization
	void Start () {
        perfecthitpositions = new Transform[5];
        perfecthitpositions[0] = GameObject.Find("LeftBullet").transform;
        perfecthitpositions[1] = GameObject.Find("LeftRay").transform;
        perfecthitpositions[2] = GameObject.Find("RightBullet").transform;
        perfecthitpositions[3] = GameObject.Find("RightRay").transform;
        perfecthitpositions[4] = GameObject.Find("HitYellow").transform;

        score = 0;
        maxcombo = 0;
        combo = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(score);
    }

    void calScore(object[] obj)
    {
        //Debug.Log(obj[0] + " " + obj[1]);

        int index = (int)obj[0];
        Vector3 hitpoint = (Vector3)obj[1];
        switch (index)
        {
            case 1:
                if ((hitpoint - perfecthitpositions[0].position).magnitude < 1)
                {
                    //Debug.Log("Perfect");
                    score += 2 * 100 + combo*50;
                }
                else
                {
                    score += 1 * 100 + combo * 50;
                }
                break;
            case 2:
                if ((hitpoint - perfecthitpositions[1].position).magnitude < 1)
                {
                    //Debug.Log("Perfect");
                    score += 2 * 100 + combo * 50;
                }
                else
                {
                    score += 1 * 100 + combo * 50;
                }
                break;
            case 3:
                if ((hitpoint - perfecthitpositions[2].position).magnitude < 1)
                {
                    //Debug.Log("Perfect");
                    score += 2 * 100 + combo * 50;
                }
                else
                {
                    score += 1 * 100 + combo * 50;
                }
                break;
            case 4:
                if ((hitpoint - perfecthitpositions[3].position).magnitude < 1)
                {
                    //Debug.Log("Perfect");
                    score += 2 * 100 + combo * 50;
                }
                else
                {
                    score += 1 * 100 + combo * 50;
                }
                break;
            case 5:
                if ((hitpoint - perfecthitpositions[4].position).magnitude < 1)
                    score += 1 * 25;
                break;
        }
        //Debug.Log("score: " + score);
    }

    void calCombo(bool comboing)
    {
        if (!comboing)
        {
            if(combo > maxcombo)
            {
                maxcombo = combo;
            }
            combo = 0;
        }
        else
        {
            combo++;
        }
        //Debug.Log("combo: " + combo);
    }
}
