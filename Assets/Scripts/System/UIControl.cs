using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class UIControl : MonoBehaviour {

    public LevelStateManager stateManager;

    //public GameObject MenuObject;
    public GameObject SongSelectionObject;
    public GameObject DifficultySelectionObject;
    public Text[] myText;
    public int songIdx = 0;
    public int songDifficulty = 0;

    public AudioSource myAudio;
    public SongListLoader songListLoader;
    public bool songPlayed = false;
    public DrumScript[] _DrumScript;
    public string currentSongScore;

    public MonsterGenerator monsterGenerator;
    public Material CanvasRender;
    public Text[] songDifficultyText;
    public Text songDifficultyLevelText;

    public int currentSelectingSongTypeId = 0;
    public string currentSelectingSongTypeString;
    public int currentSelectingSongCanvasId = 0;
    public int rightSelectingSongCanvasId = 1;
    public int leftSelectingSongCanvasId = 3;

    private SongListItem _selectedSong;
    private bool SongSelectAniLock;
    private Color[] canvasColor;
    [SerializeField]
    private Color[] solidColor = { new Color32(0x34, 0x56, 0xF6, 0xFF), new Color32(0xE1, 0x41, 0x58, 0xFF), new Color32(0xA7, 0x7C, 0x1F, 0xFF), new Color32(0x8C, 0x2E, 0xDF, 0xFF), new Color32(0x21, 0x92, 0x22, 0xFF), new Color32(0x1F, 0xBC, 0xCA, 0xFF) };

    // Use this for initialization
    void Start () {
        _DrumScript =  new DrumScript[4];
        _DrumScript[0] = GameObject.Find("BlueDrum1").GetComponent<DrumScript>();
        _DrumScript[1] = GameObject.Find("RedDrum1").GetComponent<DrumScript>();
        _DrumScript[2] = GameObject.Find("BlueDrum2").GetComponent<DrumScript>();
        _DrumScript[3] = GameObject.Find("RedDrum2").GetComponent<DrumScript>();

        myText = new Text[4];
        myText[0] = GameObject.FindGameObjectWithTag("SongText0").GetComponent<Text>();
        myText[1] = GameObject.FindGameObjectWithTag("SongText1").GetComponent<Text>();
        myText[2] = GameObject.FindGameObjectWithTag("SongText2").GetComponent<Text>();
        myText[3] = GameObject.FindGameObjectWithTag("SongText3").GetComponent<Text>();

        //songDifficultyText[songDifficulty].fontSize = 42;
        //songDifficultyText[songDifficulty].color = new Color32(0xD6, 0xD8, 0x62, 0xFF);

        songListLoader.LoadSongList();
        canvasColor = new Color[songListLoader.songTypeList.Count];
        for (int i = 0; i < songListLoader.songTypeList.Count; i++)
            canvasColor[i] = solidColor[i];
        SongSelectAniLock = true;
    }
	
	// Update is called once per frame
	void Update () {
        switch (stateManager.CurrentState)
        {
            case LevelStateManager.LevelState.Loading:
                myText[0].text = String.Format(
                    "Loading...\n{0}\n{1}",
                    songListLoader.GetLoadingSongName(), songListLoader.GetLoadingProgressString()
                );
                myText[1].text = String.Format(
                    "Loading...\n{0}\n{1}",
                    songListLoader.GetLoadingSongName(), songListLoader.GetLoadingProgressString()
                );
                myText[2].text = String.Format(
                    "Loading...\n{0}\n{1}",
                    songListLoader.GetLoadingSongName(), songListLoader.GetLoadingProgressString()
                );
                myText[3].text = String.Format(
                    "Loading...\n{0}\n{1}",
                    songListLoader.GetLoadingSongName(), songListLoader.GetLoadingProgressString()
                );
                break;
            case LevelStateManager.LevelState.DifficultySelection:
                if(!DifficultySelectionObject.activeSelf)
                {
                    DifficultySelectionObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.N))
                {//select song easy,normal,hard,ghost
                    SelectSongDifficultyLeft();
                }
                if (Input.GetKeyDown(KeyCode.M))
                {//select song easy,normal,hard,ghost
                    SelectSongDifficultyRight();
                }
                break;
            case LevelStateManager.LevelState.SongSelection:
                DifficultySelectionObject.SetActive(false);

                /*debug change song type*/
                if (Input.GetKeyDown(KeyCode.X))
                {
                    ChangeSongTypeLeft();
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    ChangeSongTypeRight();
                }

                /*debug change song*/
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ChangeSongLeft();
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    ChangeSongRight();
                }
                break;

            case LevelStateManager.LevelState.InGame:
                DifficultySelectionObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void SetSelectedSong(int songID)
    {
        _selectedSong = songListLoader.songList[songID];

        //currentSongScore = _selectedSong.GetScoreText();
        //myText.text = _selectedSong.name + "\nTop score:" + currentSongScore;
        currentSelectingSongTypeString = _selectedSong.type;
        currentSelectingSongTypeId = songListLoader.songTypeList.FindIndex(x => x == currentSelectingSongTypeString);

        CanvasRender.SetColor("_Color", canvasColor[currentSelectingSongTypeId]);

        myAudio.clip = _selectedSong.clip;
        myAudio.Play();
    }

    public void SetSelectedType(int typeID)
    {
        string tmp_currentType = songListLoader.songTypeList[typeID];
        songIdx = songListLoader.songList.FindIndex(x => x.type == tmp_currentType);
    }

    public void SetSelectedDifficulty(int diffID)
    {
        if (monsterGenerator.difficulties[diffID] != null)
        {
            if (monsterGenerator.difficulties[diffID].level != 0)
            {
                songDifficultyText[diffID].fontSize = 42;
                songDifficultyText[diffID].color = new Color32(0xD6, 0xD8, 0x62, 0xFF);
                songDifficultyLevelText.text = new String('★', monsterGenerator.difficulties[diffID].level);
            }
            else
            {
                songDifficultyText[diffID].fontSize = 42;
                songDifficultyText[diffID].color = new Color32(0x9E, 0x9E, 0x9E, 0xFF);
                songDifficultyLevelText.text = "";
            }
        }
        else
        {
            songDifficultyText[diffID].fontSize = 42;
            songDifficultyText[diffID].color = new Color32(0x9E, 0x9E, 0x9E, 0xFF);
            songDifficultyLevelText.text = "";
        }
    }

    /* Change song */
    public void ChangeSongRight()
    {
        if (SongSelectAniLock)
        {
            SongSelectAniLock = false;
            iTween.RotateBy(SongSelectionObject, iTween.Hash(
                "y", -.25,
                "easyType", "easeInOutBack",
                "oncomplete", "ChangeSongIdxRight",
                "time", .25,
                "delay", .25,
                "oncompletetarget", gameObject));
        }
    }

    void ChangeSongIdxRight()
    {
        songIdx++;
        if (songIdx >= songListLoader.songList.Count)
            songIdx = 0;

        SetSelectedSong(songIdx);

        int rightSongIdx = songIdx + 1;
        if (rightSongIdx >= songListLoader.songList.Count)
            rightSongIdx = 0;

        leftSelectingSongCanvasId = currentSelectingSongCanvasId;
        currentSelectingSongCanvasId = rightSelectingSongCanvasId;
        rightSelectingSongCanvasId++;
        if (rightSelectingSongCanvasId >= 4)
        {
            rightSelectingSongCanvasId = 0;
        }
        myText[rightSelectingSongCanvasId].text =
                (rightSongIdx + 1) + ": " + songListLoader.songList[rightSongIdx].name + "\n[" + songListLoader.songList[rightSongIdx].type + "]\nTop score:" + songListLoader.songList[rightSongIdx].GetScoreText();
        
        SongSelectAniLock = true;
    }

    public void ChangeSongLeft()
    {
        if (SongSelectAniLock)
        {
            SongSelectAniLock = false;
            iTween.RotateBy(SongSelectionObject, iTween.Hash(
                "y", .25,
                "easyType", "easeInOutBack",
                "oncomplete", "ChangeSongIdxLeft",
                "time", .25,
                "delay", .25,
                "oncompletetarget", gameObject));
        }
    }

    void ChangeSongIdxLeft()
    {
        songIdx--;
        if (songIdx < 0)
            songIdx = songListLoader.songList.Count-1;

        SetSelectedSong(songIdx);

        int leftSongIdx = songIdx - 1;
        if (leftSongIdx < 0)
            leftSongIdx = songListLoader.songList.Count - 1;

        rightSelectingSongCanvasId = currentSelectingSongCanvasId;
        currentSelectingSongCanvasId = leftSelectingSongCanvasId;
        leftSelectingSongCanvasId--;
        if (leftSelectingSongCanvasId < 0)
        {
            leftSelectingSongCanvasId = 3;
        }
        myText[leftSelectingSongCanvasId].text =
                (leftSongIdx + 1) + ": " + songListLoader.songList[leftSongIdx].name + "\n[" + songListLoader.songList[leftSongIdx].type + "]\nTop score:" + songListLoader.songList[leftSongIdx].GetScoreText();

        //myText[leftSelectingSongCanvasId].text = string.Format("-{0}-\n{1}\n[{2}]]\nTop score:{3}", (leftSongIdx + 1), songListLoader.songList[leftSongIdx].name, songListLoader.songList[leftSongIdx].type, songListLoader.songList[leftSongIdx].GetScoreText());
        SongSelectAniLock = true;
    }

    /* Change song type */
    public void ChangeSongTypeRight()
    {
        if (SongSelectAniLock)
        {
            SongSelectAniLock = false;
            iTween.RotateBy(SongSelectionObject, iTween.Hash(
                "y", -.25,
                "easyType", "easeInOutBack",
                "oncomplete", "ChangeSongTypeIdxRight",
                "time", .25,
                "delay", .25,
                "oncompletetarget", gameObject));
        }
    }

    void ChangeSongTypeIdxRight()
    {
        currentSelectingSongTypeId++;
        if (currentSelectingSongTypeId >= songListLoader.songTypeList.Count)
            currentSelectingSongTypeId = 0;

        SetSelectedType(currentSelectingSongTypeId);
        SetSelectedSong(songIdx);

        int rightSongIdx = songIdx + 1;
        if (rightSongIdx >= songListLoader.songList.Count)
            rightSongIdx = 0;
        int leftSongIdx = songIdx - 1;
        if (leftSongIdx < 0)
            leftSongIdx = songListLoader.songList.Count - 1;


        myText[rightSelectingSongCanvasId].text =
            (songIdx + 1) + ": " + songListLoader.songList[songIdx].name + "\n[" + songListLoader.songList[songIdx].type + "]\nTop score:" + songListLoader.songList[songIdx].GetScoreText();

        leftSelectingSongCanvasId = currentSelectingSongCanvasId;
        currentSelectingSongCanvasId = rightSelectingSongCanvasId;
        rightSelectingSongCanvasId++;
        if (rightSelectingSongCanvasId >= 4)
        {
            rightSelectingSongCanvasId = 0;
        }
        myText[rightSelectingSongCanvasId].text =
            (rightSongIdx + 1) + ": " + songListLoader.songList[rightSongIdx].name + "\n[" + songListLoader.songList[rightSongIdx].type + "]\nTop score:" + songListLoader.songList[rightSongIdx].GetScoreText();
        myText[leftSelectingSongCanvasId].text =
            (leftSongIdx + 1) + ": " + songListLoader.songList[leftSongIdx].name + "\n[" + songListLoader.songList[leftSongIdx].type + "]\nTop score:" + songListLoader.songList[leftSongIdx].GetScoreText();

        SongSelectAniLock = true;
    }

    public void ChangeSongTypeLeft()
    {
        if (SongSelectAniLock)
        {
            SongSelectAniLock = false;
            iTween.RotateBy(SongSelectionObject, iTween.Hash(
                "y", .25,
                "easyType", "easeInOutBack",
                "oncomplete", "ChangeSongTypeIdxLeft",
                "time", .25,
                "delay", .25,
                "oncompletetarget", gameObject));
        }
    }

    void ChangeSongTypeIdxLeft()
    {
        currentSelectingSongTypeId--;
        if (currentSelectingSongTypeId < 0 )
            currentSelectingSongTypeId = songListLoader.songTypeList.Count-1;

        SetSelectedType(currentSelectingSongTypeId);
        SetSelectedSong(songIdx);

        int leftSongIdx = songIdx - 1;
        if (leftSongIdx < 0)
            leftSongIdx = songListLoader.songList.Count - 1;
        int rightSongIdx = songIdx + 1;
        if (rightSongIdx >= songListLoader.songList.Count)
            rightSongIdx = 0;

        myText[leftSelectingSongCanvasId].text =
            (songIdx + 1) + ":" + songListLoader.songList[songIdx].name + "\n[" + songListLoader.songList[songIdx].type + "]\nTop score:" + songListLoader.songList[songIdx].GetScoreText();


        rightSelectingSongCanvasId = currentSelectingSongCanvasId;
        currentSelectingSongCanvasId = leftSelectingSongCanvasId;
        leftSelectingSongCanvasId--;
        if (leftSelectingSongCanvasId < 0)
            leftSelectingSongCanvasId = 3;
        myText[leftSelectingSongCanvasId].text =
            (leftSongIdx + 1) + ": " + songListLoader.songList[leftSongIdx].name + "\n[" + songListLoader.songList[leftSongIdx].type + "]\nTop score:" + songListLoader.songList[leftSongIdx].GetScoreText();
        myText[rightSelectingSongCanvasId].text =
            (rightSongIdx + 1) + ": " + songListLoader.songList[rightSongIdx].name + "\n[" + songListLoader.songList[rightSongIdx].type + "]\nTop score:" + songListLoader.songList[rightSongIdx].GetScoreText();

        SongSelectAniLock = true;
    }

    /* Change song diffculty */
    public void SelectSongDifficultyRight()
    {
        songDifficultyText[songDifficulty].fontSize = 35;
        songDifficultyText[songDifficulty].color = new Color32(0x97, 0x9A, 0x00, 0xFF);
        songDifficulty++;
        if (songDifficulty >= 4)
            songDifficulty = 0;

        SetSelectedDifficulty(songDifficulty);
    }

    public void SelectSongDifficultyLeft()
    {
        songDifficultyText[songDifficulty].color = new Color32(0x97, 0x9A, 0x00, 0xFF);
        songDifficultyText[songDifficulty].fontSize = 35;
        songDifficulty--;
        if (songDifficulty < 0)
            songDifficulty = 3;

        SetSelectedDifficulty(songDifficulty);
    }

    public void SelectBoardDissappear()
    {
        //for(int i =0;i<4;i++)
        //    _DrumScript[i].selectionMode = false;
        SongSelectionObject.SetActive(false);
        
        myAudio.Stop();
    }

    public void SelectBoardAppear()
    {
        if (CalculateScore.score > Convert.ToInt32(currentSongScore))
        {
            songListLoader.songList[songIdx].WriteScore(CalculateScore.score);
           
        }

        CalculateScore.score = 0;
        CalculateScore.combo = 0;
        //for (int i = 0; i < 4; i++)
        //    _DrumScript[i].selectionMode = true;
        SongSelectionObject.SetActive(true);

        SetSelectedSong(songIdx);
    }

    void SetSongAndPlay()
    {
        SetSelectedSong(0);
        myText[0].text =
            "1: " + songListLoader.songList[0].name + "\n[" + songListLoader.songList[0].type + "]\nTop score:" + songListLoader.songList[0].GetScoreText();
        myText[1].text =
            "2: " + songListLoader.songList[1].name + "\n[" + songListLoader.songList[1].type + "]\nTop score:" + songListLoader.songList[1].GetScoreText();
        myText[2].text =
            "3: " + songListLoader.songList[2].name + "\n[" + songListLoader.songList[2].type + "]\nTop score:" + songListLoader.songList[2].GetScoreText();
        myText[3].text =
            (songListLoader.songList.Count) + ": " + songListLoader.songList[songListLoader.songList.Count - 1].name + "\n[" + songListLoader.songList[songListLoader.songTypeList.Count - 1].type + "]\nTop score:" + songListLoader.songList[songListLoader.songTypeList.Count - 1].GetScoreText();

    }
}
