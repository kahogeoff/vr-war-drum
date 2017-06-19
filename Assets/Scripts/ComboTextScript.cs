using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTextScript : MonoBehaviour {
    public float DestroyTime = 0.45f;
    TextMesh _selfTextMesh;
	// Use this for initialization
	void Start () {
        _selfTextMesh = GetComponent<TextMesh>();
        _selfTextMesh.text = string.Format("{0} combos", CalculateScore.combo);
        Destroy(gameObject, DestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
