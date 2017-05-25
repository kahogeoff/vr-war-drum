using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGunFireControl : GunFireControl {
    public bool fired = false;
    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
        }
        /*****************/
        /* test hit drum */
        /*****************/
        if (Input.GetKeyDown(KeyCode.E))
        {
            fired = true;
        }
    }

    void FireTheGun(Vector3 gunPointPos)
    {
        Instantiate(Bullet, gunPointPos, Quaternion.identity);
        _selfAnimator.Play(AnimationName, -1, 0f);
    }
}
