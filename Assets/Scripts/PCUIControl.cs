using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCUIControl : MonoBehaviour {
    public GameObject InGameScoreBoard;
    public GameObject PausePanel;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (LevelStateManager.GlobalCurrentState)
        {
            case LevelStateManager.LevelState.InGame:
                if (!InGameScoreBoard.activeSelf)
                {
                    InGameScoreBoard.SetActive(true);
                }
                if (PausePanel.activeSelf)
                {
                    PausePanel.SetActive(false);
                }
                break;
            case LevelStateManager.LevelState.Pause:
                if (!PausePanel.activeSelf)
                {
                    PausePanel.SetActive(true);
                }
                break;
            default:
                if (InGameScoreBoard.activeSelf)
                {
                    //InGameScoreBoard.SetActive(false);
                    StartCoroutine(DelayHideUI(InGameScoreBoard, 1.5f));
                }
                if (PausePanel.activeSelf)
                {
                    PausePanel.SetActive(false);
                }
                break;
        }
	}

    IEnumerator DelayHideUI(GameObject uiObj, float time)
    {
        yield return new WaitForSeconds(time);
        uiObj.SetActive(false);
    }
}
