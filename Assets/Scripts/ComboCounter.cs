using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour {
    public GameObject ComboText;
    public Color MissHitColor = Color.gray;
    public Transform MissingMessageSpawnPoint;

	// Use this for initialization
	void Start () {
		if(MissingMessageSpawnPoint == null)
        {
            MissingMessageSpawnPoint = GameObject.Find("HitPoints").transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TextPopup(Transform trans)
    {
        Instantiate<GameObject>(ComboText, trans.position, trans.rotation);
    }
    void TextPopup(Transform trans, string text)
    {
        ComboText.GetComponent<ComboTextScript>().InitializeText(text);
        Instantiate<GameObject>(ComboText, trans.position, trans.rotation);
    }

    void TextPopup(Transform trans, string text, Color color)
    {
        ComboText.GetComponent<ComboTextScript>().InitializeText(text);
        ComboText.GetComponent<ComboTextScript>().InitializeText(text, color);
        Instantiate<GameObject>(ComboText, trans.position, trans.rotation);
    }

    //Only be called when miss the hit
    void MissngMessagePopup()
    {
        TextPopup(MissingMessageSpawnPoint, CalculateScore.PreviousHit, MissHitColor);
    }

    //Only be called when hit the target
    void HitMessagePopup(Transform trans)
    {
        Color tmp_color =
            Color.HSVToRGB(0.07f, Mathf.Clamp(CalculateScore.combo / 80.0f, 0.0f, 1.0f), 1.0f);
        string tmp_text = string.Format(CalculateScore.PreviousHit);

        if (CalculateScore.combo >= 5)
        {
            tmp_text = string.Format("{0} combos!!", CalculateScore.combo);
        }

        TextPopup(trans, tmp_text, tmp_color);
    }
}
