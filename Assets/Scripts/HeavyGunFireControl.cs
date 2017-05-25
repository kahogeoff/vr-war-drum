using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyGunFireControl : GunFireControl {
    public bool fired = false;
    public GameObject Explosion;

    [SerializeField]
    private Vector3 _testPos = new Vector3(3.6f, -1.0f, -7f);

    // Use this for initialization
    void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
        /*****************/
        /* test hit drum */
        /*****************/
        if (Input.GetKeyDown(KeyCode.R))
        {
            FireTheGun(_testPos);
        }
    }

    void FireTheGun(Vector3 gunPointPos)
    {
        Instantiate(Explosion, gunPointPos, Quaternion.identity);
        Instantiate(Bullet, gunPointPos, Quaternion.identity);
        _selfAnimator.Play(AnimationName, -1, 0f);
    }
}
