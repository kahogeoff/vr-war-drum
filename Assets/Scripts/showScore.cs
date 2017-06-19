using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class showScore : MonoBehaviour {
    public Text _text;
	// Use this for initialization
	void Start () {
        _text = GetComponent<Text>();
        _text.text = "Score:" + CalculateScore.score + "\n" + "Combo:"+ CalculateScore.combo;

    }
	
	// Update is called once per frame
	void Update () {
        _text.text = "Score:" + CalculateScore.score + "\n" + "Combo:" + CalculateScore.combo;
    }
}
