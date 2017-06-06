using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBulletControl : BulletObject
{
    // Use this for initialization
    void Start()
    {
        base.Start();
        transform.eulerAngles = new Vector3(-90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime);
    }

    //void OnTriggerEnter(Collider c)
    //{
    //    if (c.CompareTag("Monsters"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
