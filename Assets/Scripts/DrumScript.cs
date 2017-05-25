using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumScript : MonoBehaviour {
    public Transform Gunpoint;
    public GameObject Bullet;
    public GameObject Gun;
    public float AcceptableHittingForce = 0.1f;

    private bool _canFire = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ShootABullet()
    {
        if(Gun)
        {
            Gun.SendMessage("FireTheGun", Gunpoint.position);
        }
        else
        {
            Instantiate<GameObject>(Bullet, Gunpoint.position, Gunpoint.rotation);
        }
        
    }

    void EnableFiring()
    {
        _canFire = true;
    }

    void DisableFiring()
    {
        _canFire = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.relativeVelocity.magnitude);
        
        float tmp_hitForce = collision.relativeVelocity.magnitude;
        Debug.Log("Force = " + tmp_hitForce);
        if (_canFire && tmp_hitForce > AcceptableHittingForce)
        {
            gameObject.SendMessage("ShootABullet");
        }
    }
}
