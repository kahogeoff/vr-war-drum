using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateManager : MonoBehaviour {
    public enum LevelState
    {
        SongSelection, InGame, Pause, EndGame
    }

    public UIControl _UIControl;
    public DrumStickControl drumStickControl;

    [SerializeField]
    private SteamVR_TrackedObject leftController;
    [SerializeField]
    private SteamVR_TrackedObject rightController;


    private SteamVR_Controller.Device leftDevice;
    private SteamVR_Controller.Device rightDevice;

    public LevelState CurrentState = LevelState.SongSelection;

    // Use this for initialization
    void Start() {
        CurrentState = LevelState.SongSelection;
        leftDevice = SteamVR_Controller.Input((int)leftController.index);
        rightDevice = SteamVR_Controller.Input((int)rightController.index);
    }

    void FixedUpdate()
    {
        if(leftDevice == null)
            leftDevice = SteamVR_Controller.Input((int)leftController.index);

        if(rightDevice == null)
            rightDevice = SteamVR_Controller.Input((int)rightController.index);
    }

    // Update is called once per frame
    void Update() {

        if (leftDevice != null && rightDevice != null)
        {
            if (leftDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) 
                || rightDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
            {
                SendMessage("LeftStickOn");
                SendMessage("RightStickOn");
            }

            switch (CurrentState)
            {
                case LevelState.SongSelection:
                    if(drumStickControl.GetBothDrumHitted() == DrumScript.DrumType.Red || Input.GetKeyDown(KeyCode.Space))
                    {
                        GameStart();
                    }
                    break;
                case LevelState.InGame:
                    if (leftDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)
                        || rightDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)
                        || Input.GetKeyDown(KeyCode.Space))
                    {
                        GamePause();
                    }else if (leftDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)
                        || rightDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)
                        || Input.GetKeyDown(KeyCode.Escape))
                    {
                        GameEnd();
                    }
                    break;
                case LevelState.Pause:
                    if (leftDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)
                        || rightDevice.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)
                        || Input.GetKeyDown(KeyCode.Space))
                    {
                        GameResume();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void ChangeState(LevelState state)
    {
        CurrentState = state;
    }

    void GameStart()
    {
        SendMessage("PlaybackStart");
        _UIControl.SelectBoardDissappear();
        ChangeState(LevelState.InGame);
    }

    void GamePause()
    {
        SendMessage("PlaybackPause");
        ChangeState(LevelState.Pause);
    }

    void GameResume()
    {
        SendMessage("PlaybackResume");
        ChangeState(LevelState.InGame);
    }
    
    void GameEnd()
    {
        SendMessage("PlaybackEnd");
        StartCoroutine("OnGameEnd");
    }

    IEnumerator OnGameEnd()
    {
        ChangeState(LevelState.EndGame);
        yield return new WaitForSeconds(3.0f);
        _UIControl.SelectBoardAppear();
        ChangeState(LevelState.SongSelection);
    }
}
