using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour {

    //public enum EnemyType
    //{
    //    Red, Blue
    //}

    //public EnemyType Type = EnemyType.Red;

    public int myIndex;
    public float speed = 100.0f;
    public GameObject scoreCaculaor;

    private Transform[] spawnsites;
    private Transform target;

    // Use this for initialization
    void Start () {
        spawnsites = new Transform[4];
        spawnsites[0] = GameObject.FindGameObjectWithTag("SpawnSite1").transform;
        spawnsites[1] = GameObject.FindGameObjectWithTag("SpawnSite2").transform;
        spawnsites[2] = GameObject.FindGameObjectWithTag("SpawnSite3").transform;
        spawnsites[3] = GameObject.FindGameObjectWithTag("SpawnSite4").transform;

        if (transform.position == spawnsites[0].position)
        {
            myIndex = 1;
            target = GameObject.Find("LeftBullet").gameObject.transform;
        }
        else if (transform.position == spawnsites[1].position)
        {
            myIndex = 2;
            target = GameObject.Find("LeftRay").gameObject.transform;
        }
        else if (transform.position == spawnsites[2].position)
        {
            myIndex = 3;
            target = GameObject.Find("RightBullet").gameObject.transform;
        }
        else if (transform.position == spawnsites[3].position)
        {
            myIndex = 4;
            target = GameObject.Find("RightRay").gameObject.transform;
        }
        //Debug.Log(target.position);
        //target.position = new Vector3(-4.8F, 1.25F, 10F);

        Destroy(gameObject, 5.0f / (speed/ 100));

		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;

        scoreCaculaor = GameObject.Find("ScoreCalculator");

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", target.position,
            "speed",speed,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "MoveExtraForward"));
    }

    // Update is called once per frame
    void Update () {
		//transform.Translate (Vector3.forward * speed * Time.deltaTime);

        if(transform.position.z > 0.0f)
        {
            scoreCaculaor.SendMessage("calCombo", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");

        switch (myIndex)
        {
            case 1:
                if (other.CompareTag("Bullet"))
                {
                    Destroy(gameObject);
                    Destroy(other.gameObject);

                    object[] message = new object[2];
                    message[0] = myIndex;
                    message[1] = transform.position;
                    scoreCaculaor.SendMessage("calScore", message);
                    scoreCaculaor.SendMessage("calCombo", true);
                }
                break;
            case 2:
                if (other.CompareTag("Ray"))
                {
                    Destroy(gameObject);
                    Destroy(other.gameObject);

                    object[] message = new object[2];
                    message[0] = myIndex;
                    message[1] = transform.position;
                    scoreCaculaor.SendMessage("calScore", message);
                    scoreCaculaor.SendMessage("calCombo", true);
                }
                break;
            case 3:
                if (other.CompareTag("Bullet"))
                {
                    Destroy(gameObject);
                    Destroy(other.gameObject);

                    object[] message = new object[2];
                    message[0] = myIndex;
                    message[1] = transform.position;
                    scoreCaculaor.SendMessage("calScore", message);
                    scoreCaculaor.SendMessage("calCombo", true);
                }
                break;
            case 4:
                if (other.CompareTag("Ray"))
                {
                    Destroy(gameObject);
                    Destroy(other.gameObject);

                    object[] message = new object[2];
                    message[0] = myIndex;
                    message[1] = transform.position;
                    scoreCaculaor.SendMessage("calScore", message);
                    scoreCaculaor.SendMessage("calCombo", true);
                }
                break;
        }

    }
    
    void MoveExtraForward()
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", target.position + transform.forward * speed,
            "speed", speed,
            "easetype", iTween.EaseType.linear));
    }
}
