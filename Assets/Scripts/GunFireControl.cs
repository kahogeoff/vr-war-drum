using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireControl : MonoBehaviour {
    public GameObject Bullet;
    public string AnimationName = "";

    [SerializeField] protected Animator _selfAnimator;
    // Use this for initialization
    protected void Start () {
        _selfAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected void Update () {
		
	}
}
