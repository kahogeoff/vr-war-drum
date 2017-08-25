using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ControllerInputManager))]
public class LevelStateManager : MonoBehaviour {
    public enum LevelState
    {
        Loading,
        NameSelection,
        SongSelection,
        DifficultySelection,
        InGame,
        Pause,
        ResultDisplay,
        EndGame
    }

    public UIControl _UIControl;
    public DrumControl drumControl;
    public DrumStickControl drumStickControl;

    public LevelState CurrentState = LevelState.SongSelection;
    static public LevelState GlobalCurrentState = LevelState.SongSelection;

    private ControllerInputManager _controllerInput;

    // Use this for initialization
    void Start() {
        CurrentState = LevelState.Loading;

        _controllerInput = GetComponent<ControllerInputManager>();
    }
    

    // Update is called once per frame
    void Update() {
        GlobalCurrentState = CurrentState;
        if (_controllerInput.CheckControllerValid())
        {            
            if (_controllerInput.CheckAnyPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
            {
                SendMessage("LeftStickOn");
                SendMessage("RightStickOn");
            }

        }

        switch (CurrentState)
        {
            case LevelState.SongSelection:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //GameStart();
                    SongSelected();
                }
                break;

            case LevelState.DifficultySelection:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TryGameStart();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SongDeselected();
                }
                break;

            case LevelState.InGame:

                if (_controllerInput.CheckAnyPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)
                    || Input.GetKeyDown(KeyCode.Escape))
                {
                    GameEnd();
                    CleanScreen();
                }
                else if (_controllerInput.CheckAnyPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)
                    || Input.GetKeyDown(KeyCode.Space))
                {
                    GamePause();
                }
                break;

            case LevelState.Pause:

                if (_controllerInput.CheckAnyPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)
                    || Input.GetKeyDown(KeyCode.Escape))
                {
                    GameResume();
                    GameEnd();
                    CleanScreen();
                }
                else if (_controllerInput.CheckAnyPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)
                    || Input.GetKeyDown(KeyCode.Space))
                {
                    GameResume();
                }
                break;

            default:
                break;
        }
    }

    void FixedUpdate()
    {

        switch (CurrentState)
        {
            case LevelState.SongSelection:
                if (drumStickControl.GetBothDrumHitted() == DrumScript.DrumType.Red/* || Input.GetKeyDown(KeyCode.Space)*/)
                {
                    //GameStart();
                    SongSelected();
                }
                else {
                    if (drumStickControl.GetLeftStickHitStatus().x == 1 || drumStickControl.GetRightStickHitStatus().x == 1)
                    {
                        if(drumStickControl.GetLeftStickHitStatus().y == 1 || drumStickControl.GetRightStickHitStatus().y == 1)
                        {
                            _UIControl.ChangeSongLeft();
                        }
                        else if(drumStickControl.GetLeftStickHitStatus().y == 2 || drumStickControl.GetRightStickHitStatus().y == 2)
                        {
                            _UIControl.ChangeSongTypeLeft();
                        }
                    }else if (drumStickControl.GetLeftStickHitStatus().x == 2 || drumStickControl.GetRightStickHitStatus().x == 2)
                    {
                        if (drumStickControl.GetLeftStickHitStatus().y == 1 || drumStickControl.GetRightStickHitStatus().y == 1)
                        {
                            _UIControl.ChangeSongRight();
                        }
                        else if (drumStickControl.GetLeftStickHitStatus().y == 2 || drumStickControl.GetRightStickHitStatus().y == 2)
                        {
                            _UIControl.ChangeSongTypeRight();
                        }
                    }
                }
                break;
                
            case LevelState.DifficultySelection:
                if (drumStickControl.GetBothDrumHitted() == DrumScript.DrumType.Red/* || Input.GetKeyDown(KeyCode.Space)*/)
                {
                    TryGameStart();
                }
                else if (drumStickControl.GetBothDrumHitted() == DrumScript.DrumType.Blue /*|| Input.GetKeyDown(KeyCode.Escape)*/)
                {
                    SongDeselected();
                }
                else
                {
                    if (drumStickControl.GetLeftStickHitStatus().x == 1 || drumStickControl.GetRightStickHitStatus().x == 1)
                    {
                        _UIControl.SelectSongDifficultyLeft();
                    }
                    else if (drumStickControl.GetLeftStickHitStatus().x == 2 || drumStickControl.GetRightStickHitStatus().x == 2)
                    {
                        _UIControl.SelectSongDifficultyRight();
                    }
                }
                break;

            default:
                break;
        }
    }

    void ChangeState(LevelState state)
    {
        CurrentState = state;
    }
    void TryGameStart()
    {
        SendMessage("PlaybackStart");
    }

    void GameStart()
    {
        foreach (Text t in _UIControl.songDifficultyText)
        {
            t.fontSize = 35;
            t.color = new Color32(0x97, 0x9A, 0x00, 0xFF);
        }

        _UIControl.SelectBoardDissappear();
        ChangeState(LevelState.InGame);
    }

    void SongDeselected()
    {
        foreach (Text t in _UIControl.songDifficultyText)
        {
            t.fontSize = 35;
            t.color = new Color32(0x97, 0x9A, 0x00, 0xFF);
        }

        SendMessage("UnloadTheSong");
        ChangeState(LevelState.SongSelection);
    }

    void SongSelected()
    {
        SendMessage("PrepareTheSong");
        ChangeState(LevelState.DifficultySelection);
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
    void CleanScreen()
    {
        GameObject[] tmp_monsterList = GameObject.FindGameObjectsWithTag("Monsters");
        foreach (GameObject monster in tmp_monsterList)
        {
            Destroy(monster);
        }
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
