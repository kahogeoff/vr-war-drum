using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public float MoveSpeed = 50.0f;
    public AudioClip audioClip;

    // Use this for initialization
    protected void Start ()
    {
        GameObject audio = new GameObject();
        audio.AddComponent<AudioSource>();
        audio.GetComponent<AudioSource>().clip = audioClip;
        audio.GetComponent<AudioSource>().playOnAwake = true;
        audio.GetComponent<AudioSource>().volume = 0.3f;
        audio.GetComponent<AudioSource>().Play();

        Destroy(audio, 5.0f);
        Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    protected void Update () {
		
	}
}
