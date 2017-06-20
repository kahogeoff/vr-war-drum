using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour {

    //public enum EnemyType
    //{
    //    Red, Blue
    //}

    //public EnemyType Type = EnemyType.Red;

    static public bool continuously_beating = false;
    public int myIndex;
    public float speed = 100.0f;
    public GameObject scoreCaculaor;
    public GameObject destroyEffect;
    public GameObject HitNotice;
    public Vector3 EffectScale = Vector3.one;
    public Vector2 HitNoticeRange = new Vector2(3.0f, 0.8f);

    private Transform[] spawnsites;
    private Transform target;

    // Use this for initialization
    void Start () {
        spawnsites = new Transform[5];
        spawnsites[0] = GameObject.FindGameObjectWithTag("SpawnSite1").transform;
        spawnsites[1] = GameObject.FindGameObjectWithTag("SpawnSite2").transform;
        spawnsites[2] = GameObject.FindGameObjectWithTag("SpawnSite3").transform;
        spawnsites[3] = GameObject.FindGameObjectWithTag("SpawnSite4").transform;
        spawnsites[4] = GameObject.FindGameObjectWithTag("SpawnSite567").transform;

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
        else if (transform.position == spawnsites[4].position)
        {
            myIndex = 5;
            target = GameObject.Find("HitYellow").transform;
        }
        //Debug.Log(target.position);
        //target.position = new Vector3(-4.8F, 1.25F, 10F);

		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;

        scoreCaculaor = GameObject.Find("ScoreCalculator");

        if(HitNotice != null)
        {
            GameObject tmp_HitNotice = Instantiate<GameObject>(HitNotice, transform);
            float tmp_time = Vector3.Distance(target.position, transform.position)/ speed;
            tmp_HitNotice.transform.localScale = Vector3.one * HitNoticeRange.x;
            iTween.ScaleTo(tmp_HitNotice, iTween.Hash(
                "scale", Vector3.one * HitNoticeRange.y,
                "time", tmp_time,
                "easetype", iTween.EaseType.linear));
        }

        if (myIndex != 5)
        {
            Destroy(gameObject, 5.0f / (speed / 100));
            iTween.MoveTo(gameObject, iTween.Hash(
            "position", target.position,
            "speed", speed,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "MoveExtraForward"));
        }
        else
        {
            ToggleCollider(false);
            iTween.MoveTo(gameObject, iTween.Hash(
            "position", target.position,
            "speed", speed,
            "easetype", iTween.EaseType.linear,//));
            "oncomplete", "ToggleCollider",
            "oncompleteparams", true));
        }
    }

    // Update is called once per frame
    void Update () {
        //transform.Translate (Vector3.forward * speed * Time.deltaTime);
        Debug.Log(continuously_beating);
        if(transform.position.z > 0.0f)
        {
            scoreCaculaor.SendMessage("calCombo", false);
        }
        MoveAway();

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
            case 5:
                if (other.CompareTag("Ray") || other.CompareTag("Bullet"))
                {
                    //Destroy(other.gameObject);

                    object[] message = new object[2];
                    message[0] = myIndex;
                    message[1] = transform.position;
                    scoreCaculaor.SendMessage("calScore", message);
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

    void OnDestroy()
    {
        //Instantiate(destroyEffect, this.transform);
        GameObject.FindGameObjectWithTag("LevelManager").SendMessage("TextPopup", transform);

        GameObject tmp_destroyEffect = Instantiate(destroyEffect, transform.position, transform.rotation);
        tmp_destroyEffect.transform.localScale = EffectScale;
        Destroy(tmp_destroyEffect, .5f);

    }

    void MoveAway()
    {
        //Debug.Log("=====MoveAway=====");
        //Debug.Log(continuously_beating);
        if (myIndex == 5 && continuously_beating)
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
            continuously_beating = false;
        }
    }

    void ToggleCollider(bool state)
    {
        GetComponent<Collider>().enabled = state;
    }
}
