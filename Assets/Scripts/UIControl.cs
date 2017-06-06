using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {
    public GameObject UIobject;
    public GameObject UIobject2;
    public Text myText;
    public int songIdx = 0;
    public List<string> songList;
    public List<string> songFullList;
    public List<string> songDirectList;
    public AudioSource myAudio;
    public bool songPlayed = false;
    public DrumScript[] _DrumScript;

    // Use this for initialization
    void Start () {
        _DrumScript =  new DrumScript[4];
        _DrumScript[0] = GameObject.Find("BlueDrum1").GetComponent<DrumScript>();
        _DrumScript[1] = GameObject.Find("RedDrum1").GetComponent<DrumScript>();
        _DrumScript[2] = GameObject.Find("BlueDrum2").GetComponent<DrumScript>();
        _DrumScript[3] = GameObject.Find("RedDrum2").GetComponent<DrumScript>();
        songList = new List<string>();
        songFullList = new List<string>();
        songDirectList = new List<string>();
        string path = "Assets/Resources/songs/";
        var info = new DirectoryInfo(path);
        var DirectoryInfo = info.GetDirectories();
        foreach (var directory in DirectoryInfo)
        {
            Debug.Log("Directory: " + directory.Name);
            var subInfo = new DirectoryInfo(path + directory.Name);
            var subDirectoryInfo = subInfo.GetDirectories();
            foreach (var subDirectory in subDirectoryInfo)
            {
                //Debug.Log("\t songDirectory: " + subDirectory.Name);
                songList.Add(subDirectory.Name);
                //Debug.Log("songs/" + directory.Name + "/" + subDirectory.Name + "/" + subDirectory.Name);
                songFullList.Add("songs/" + directory.Name + "/" + subDirectory.Name+"/"+ subDirectory.Name);

                songDirectList.Add("Assets/Resources/songs/" + directory.Name + "/" + subDirectory.Name);
                Debug.Log("Assets/Resources/songs/" + directory.Name + "/" + subDirectory.Name);
                

            }
        }
        Debug.Log("songs number: " + songDirectList.Count);

        /*StreamWriter writer = new StreamWriter(songDirectList[0] + "/score.txt");
        writer.WriteLine("0");
        writer.Close();
        */
        StreamReader scoreReader = new StreamReader(songDirectList[songIdx] + "/score.txt");
        myText.text = songList[songIdx] + "\nTop score:" + scoreReader.ReadLine();
        scoreReader.Close();
        myAudio.clip = Resources.Load<AudioClip>(songFullList[songIdx]);
        myAudio.Play();
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

        /*debug comfirm song
        if (Input.GetAxis("LeftTrigger") + Input.GetAxis("RightTrigger") > 1.8f)
        {
            SelectBoardDissappear();
        }
        */
    }

    public void ChangeSongRight()
    {
        songIdx++;
        if (songIdx == songList.Count)
            songIdx = 0;
        StreamReader scoreReader = new StreamReader(songDirectList[songIdx] + "/score.txt");
        myText.text = songList[songIdx] + "\nTop score:" + scoreReader.ReadLine();
        scoreReader.Close();
        myAudio.clip = Resources.Load<AudioClip>(songFullList[songIdx]);
        myAudio.Play();
    }

    public void ChangeSongLeft()
    {
        songIdx--;
        if (songIdx == -1)
            songIdx = songList.Count-1;
        StreamReader scoreReader = new StreamReader(songDirectList[songIdx] + "/score.txt");
        myText.text = songList[songIdx] + "\nTop score:" + scoreReader.ReadLine();
        scoreReader.Close();
        myAudio.clip = Resources.Load<AudioClip>(songFullList[songIdx]);
        myAudio.Play();
    }

    public void SelectBoardDissappear()
    {
        for(int i =0;i<4;i++)
            _DrumScript[i].selectionMode = false;
        UIobject2.SetActive(false);
        myAudio.Stop();
        Debug.Log(songList[songIdx]);
    }

    public void SelectBoardAppear()
    {
        for (int i = 0; i < 4; i++)
            _DrumScript[i].selectionMode = true;
        UIobject2.SetActive(true);
        StreamReader scoreReader = new StreamReader(songDirectList[songIdx] + "/score.txt");
        myText.text = songList[songIdx] + "\nTop score:" + scoreReader.ReadLine();
        scoreReader.Close();
        myAudio.clip = Resources.Load<AudioClip>(songFullList[songIdx]);
        myAudio.Play();
    }
}
