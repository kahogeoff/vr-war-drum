using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollow : MonoBehaviour {
    public Transform Target;
    public float ReturnSpeed = 1.0f;
    public float Range = 1.0f;

    [SerializeField] SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    private Rigidbody _selfRigidbody;
    bool _lock = false;
	// Use this for initialization
	void Start () {
        _selfRigidbody = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    void Update () {
        Vector3 tmp_heading = Target.position - transform.position;
        float tmp_distance = tmp_heading.magnitude;
        Vector3 tmp_dir = tmp_heading / tmp_distance;

        if (tmp_heading.sqrMagnitude > Range * Range)
        {
            _selfRigidbody.velocity = device.velocity + (tmp_dir * ReturnSpeed); //transform.forward;
            _selfRigidbody.angularVelocity = device.angularVelocity;

        }
        else if(!_lock){ _selfRigidbody.velocity = device.velocity; }

    }

    void OnCollisionEnter(Collision c)
    {
        _selfRigidbody.velocity = Vector3.zero;
        Debug.Log(c.collider.tag.Contains("Drum"));
        if(c.collider.tag.Contains("Drum"))
        {
            device.TriggerHapticPulse(1000);
        }
        _lock = true;
    }
    
    void OnCollisionStay(Collision c)
    {
        _selfRigidbody.velocity = Vector3.zero;
    }

    void OnCollisionExit(Collision c)
    {
        _lock = false;
    }
}
