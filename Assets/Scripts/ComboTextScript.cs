using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTextScript : MonoBehaviour {
    public float DestroyTime = 0.45f;
    public string DisplayText = "Hit!";
    public Color DisplayColor = Color.white;

    [SerializeField]
    private TextMesh _selfTextMesh;

	// Use this for initialization
	void Start () {
        _selfTextMesh = GetComponent<TextMesh>();

        Destroy(gameObject, DestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeText(string text)
    {
        DisplayText = text;
        _selfTextMesh.text = DisplayText;
    }

    public void InitializeText(string text, Color color)
    {
        DisplayText = text;
        DisplayColor = color;

        _selfTextMesh.text = DisplayText;
        _selfTextMesh.color = DisplayColor;
    }
}
