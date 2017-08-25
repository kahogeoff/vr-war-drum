using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireControl : MonoBehaviour {
    public GameObject Bullet;
    public string AnimationName = "";
    public KeyCode DebugKey;

    [SerializeField] protected Animator _selfAnimator;

    [SerializeField] protected Transform _testTransform;

    [SerializeField]
    protected Vector3 _testPos = new Vector3(3.6f, -1.0f, -7f);

    // Use this for initialization
    protected void Start () {
        _selfAnimator = GetComponent<Animator>();
        if (_testTransform != null)
        {
            _testPos = _testTransform.position;
        }
    }

    // Update is called once per frame
    protected void Update () {
		
	}
}
