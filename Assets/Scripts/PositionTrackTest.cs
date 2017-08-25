using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTrackTest : MonoBehaviour
{
    public Transform Origin;
    public Vector3 PositionOffset = Vector3.zero;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Origin.position + PositionOffset;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Origin.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}