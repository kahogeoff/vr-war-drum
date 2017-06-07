using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSite : MonoBehaviour {

    public float flyingtime = 4.0f;

    private Transform[] hitpositions;
    
    // Use this for initialization
    void Start () {
        hitpositions = new Transform[5];
        hitpositions[0] = GameObject.Find("LeftBullet").transform;
        hitpositions[1] = GameObject.Find("LeftRay").transform;
        hitpositions[2] = GameObject.Find("RightBullet").transform;
        hitpositions[3] = GameObject.Find("RightRay").transform;
        hitpositions[4] = GameObject.Find("HitYellow").transform;

        flyingtime = 4.0f;
        if (gameObject.name == "SpawnSite1")
        {
            Vector3 direction = new Vector3(60.0f, 100.0f, -400.0f);
            transform.position = hitpositions[0].position + direction.normalized * 100.0f * flyingtime;
        }
        else if (gameObject.name == "SpawnSite2")
        {
            Vector3 direction = new Vector3(200.0f, 80.0f, -400.0f);
            transform.position = hitpositions[1].position + direction.normalized * 100.0f * flyingtime;
        }
        else if (gameObject.name == "SpawnSite3")
        {
            Vector3 direction = new Vector3(-60.0f, 100.0f, -400.0f);
            transform.position = hitpositions[2].position + direction.normalized * 100.0f * flyingtime;
        }
        else if (gameObject.name == "SpawnSite4")
        {
            Vector3 direction = new Vector3(-200.0f, 80.0f, -400.0f);
            transform.position = hitpositions[3].position + direction.normalized * 100.0f * flyingtime;
        }
        else if (gameObject.name == "SpawnSite567")
        {
            Vector3 direction = new Vector3(0.0f, 0.0f, -1.0f);
            transform.position = hitpositions[4].position + direction * 100.0f * flyingtime;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
