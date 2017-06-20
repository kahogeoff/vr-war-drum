using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTextScript : MonoBehaviour {
    public float DestroyTime = 0.45f;
    TextMesh _selfTextMesh;
	// Use this for initialization
	void Start () {
        _selfTextMesh = GetComponent<TextMesh>();
        if(CalculateScore.combo >= 5)
        {
            _selfTextMesh.text = string.Format("{0} combos!!", CalculateScore.combo);
        }
        else
        {
            _selfTextMesh.text = string.Format("Hit!");
        }
        _selfTextMesh.color = Color.HSVToRGB(0.07f, Mathf.Clamp(CalculateScore.combo / 80.0f, 0.0f, 1.0f),1.0f);

        Destroy(gameObject, DestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
