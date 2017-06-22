using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class UIControl : MonoBehaviour {
    public GameObject UIobject;
    public GameObject UIobject2;
    public Text myText;
    public int songIdx = 0;

    public AudioSource myAudio;
    public SongListLoader songListLoader;
    public bool songPlayed = false;
    public DrumScript[] _DrumScript;
    public string currentSongScore;

    // Use this for initialization
    void Start () {
        _DrumScript =  new DrumScript[4];
        _DrumScript[0] = GameObject.Find("BlueDrum1").GetComponent<DrumScript>();
        _DrumScript[1] = GameObject.Find("RedDrum1").GetComponent<DrumScript>();
        _DrumScript[2] = GameObject.Find("BlueDrum2").GetComponent<DrumScript>();
        _DrumScript[3] = GameObject.Find("RedDrum2").GetComponent<DrumScript>();
        
        songListLoader.LoadSongList();
    }
	
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(UIobject.activeSelf)
                UIobject.SetActive(false);
            else
                UIobject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (UIobject2.activeSelf)
                UIobject2.SetActive(false);
            else
                UIobject2.SetActive(true);
        }
        /*debug change song*/
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeSongLeft();
        }
        /*debug change song*/
        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeSongRight();
        }
    }

    public void ChangeSongRight()
    {
        songIdx++;
        if (songIdx >= songListLoader.songList.Count)
            songIdx = 0;
       
        currentSongScore = songListLoader.songList[songIdx].GetScoreText();
        myText.text = songListLoader.songList[songIdx].name + "\nTop score:" + currentSongScore;
        myAudio.clip = songListLoader.songList[songIdx].clip;
        myAudio.Play();
    }

    public void ChangeSongLeft()
    {
        songIdx--;
        if (songIdx < 0)
            songIdx = songListLoader.songList.Count-1;

        currentSongScore = songListLoader.songList[songIdx].GetScoreText();
        myText.text = songListLoader.songList[songIdx].name + "\nTop score:" + currentSongScore;
        myAudio.clip = songListLoader.songList[songIdx].clip;
        myAudio.Play();
    }

    public void SelectBoardDissappear()
    {
        for(int i =0;i<4;i++)
            _DrumScript[i].selectionMode = false;
        UIobject2.SetActive(false);

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
        for (int i = 0; i < 4; i++)
            _DrumScript[i].selectionMode = true;
	    
        UIobject2.SetActive(true);

        currentSongScore = songListLoader.songList[songIdx].GetScoreText();
        myText.text = songListLoader.songList[songIdx].name + "\nTop score:" + currentSongScore;
        myAudio.clip = songListLoader.songList[songIdx].clip;
        myAudio.Play();
    }

    void SetSongAndPlay()
    {
        currentSongScore = songListLoader.songList[songIdx].GetScoreText();
        myText.text = songListLoader.songList[songIdx].name + "\nTop score:" + currentSongScore;
        myAudio.clip = songListLoader.songList[songIdx].clip;
        myAudio.Play();
    }
   
}
