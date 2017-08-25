using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumHintsControl : MonoBehaviour {
    public GameObject[] RedDrumSideHints;
    public GameObject RedDrumCenterHint;
    public GameObject[] BlueDrumSideHints;
    public GameObject BlueDrumCenterHint;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (LevelStateManager.GlobalCurrentState)
        {
            case LevelStateManager.LevelState.SongSelection:
                foreach (GameObject g in RedDrumSideHints)
                {
                    g.SetActive(true);
                }
                RedDrumCenterHint.SetActive(true);
                foreach (GameObject g in BlueDrumSideHints)
                {
                    g.SetActive(true);
                }
                BlueDrumCenterHint.SetActive(false);

                break;
            case LevelStateManager.LevelState.DifficultySelection:
                BlueDrumCenterHint.SetActive(true);

                break;
            default:
                foreach (GameObject g in RedDrumSideHints)
                {
                    g.SetActive(false);
                }
                RedDrumCenterHint.SetActive(false);

                foreach (GameObject g in BlueDrumSideHints)
                {
                    g.SetActive(false);
                }
                BlueDrumCenterHint.SetActive(false);

                break;
        }
	}
}
