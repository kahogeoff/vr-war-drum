using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBulletControl : MonoBehaviour {
    public float MoveSpeed = 50.0f;
    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 5.0f);
        transform.eulerAngles = new Vector3(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
    }
}
