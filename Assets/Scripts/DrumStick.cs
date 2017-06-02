using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumStick : MonoBehaviour {
    public Transform Target;
    public float ReturnSpeed = 1.0f;
    public float Range = 1.0f;

    public GameObject HitEffect;
    public Vector3 EffectScale = Vector3.one;

    [SerializeField] SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    private Rigidbody _selfRigidbody;
    bool _lock = false;
	// Use this for initialization
	void Start () {
        _selfRigidbody = GetComponent<Rigidbody>();
        transform.position = trackedObj.transform.position;
    }
    void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    void Update () {
        if (device == null) { return; }
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
        //Debug.Log(c.collider.tag.Contains("Drum"));
        if(c.collider.tag.Contains("Drum"))
        {
            float tmp_acceptableForce = c.gameObject.GetComponent<DrumScript>().AcceptableHittingForce;
            if (c.relativeVelocity.magnitude > tmp_acceptableForce)
            {
                if (HitEffect)
                {
                    GameObject tmp_FX = Instantiate(HitEffect, transform.position, transform.rotation);
                    tmp_FX.transform.localScale = EffectScale;
                    Destroy(tmp_FX, .5f);
                }
                device.TriggerHapticPulse(3000);
            }
            else
            {
                device.TriggerHapticPulse(0);
            }
        }
        _lock = true;
    }
    
    void OnCollisionStay(Collision c)
    {
        _selfRigidbody.velocity = Vector3.zero;
    }

    void OnCollisionExit(Collision c)
    {
        if (c.collider.tag.Contains("Drum"))
        {
            device.TriggerHapticPulse(1000);
        }
        _lock = false;
    }
}
