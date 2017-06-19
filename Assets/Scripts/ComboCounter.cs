using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter : MonoBehaviour {
    public GameObject ComboText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TextPopup(Transform trans)
    {
        Instantiate<GameObject>(ComboText, trans.position, trans.rotation);
    }
}
