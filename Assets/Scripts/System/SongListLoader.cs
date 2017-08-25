using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TargetMessagePair
{
    public GameObject Target;
    public string MessageToSend;
}

[Serializable]
public class SongListItem
{
    public string name;
    public string type;
    public string songPath;
    public AudioClip clip;

    public string GetScoreText()
    {
        string tmp_scoreText;
        StreamReader scoreReader = new StreamReader(songPath + "/score.txt", System.Text.Encoding.UTF8);
        tmp_scoreText = scoreReader.ReadLine();
        scoreReader.Close();
        return tmp_scoreText;
    }

    public void WriteScore(int new_score)
    {
        StreamWriter scoreWriter = new StreamWriter(songPath + "/score.txt", false, System.Text.Encoding.UTF8);
        scoreWriter.WriteLine(new_score);
        scoreWriter.Close();
    }
} 

[RequireComponent(typeof(LevelStateManager))]
public class SongListLoader : MonoBehaviour {

    public string songPath = "Resources/songs/";
    public List<SongListItem> songList = new List<SongListItem>();
    public List<string> songTypeList = new List<string>();
    public AudioSource myAudio;

    public TargetMessagePair[] callBackMessages;
    //[SerializeField]

    private string _runtimePath;
    private string _loadingAudioName;
    private int _loadedAudioNumber;
    // Use this for initialization
    void Awake () {

        if (Application.platform == RuntimePlatform.WindowsEditor ||
           Application.platform == RuntimePlatform.LinuxEditor ||
           Application.platform == RuntimePlatform.OSXEditor)
        {
            _runtimePath = Application.dataPath + "/";
        }
        else
        {
            _runtimePath = Application.dataPath + "/../Assets/";
        }

        _loadedAudioNumber = 0;
        Time.timeScale = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadSongList()
    {
        songList = new List<SongListItem>();
        string tmp_pathPrefix = _runtimePath + songPath; 
        DirectoryInfo tmp_directoryInfo = new DirectoryInfo(tmp_pathPrefix);
        DirectoryInfo[] tmp_subDirectoriesInfo = tmp_directoryInfo.GetDirectories();

        foreach (DirectoryInfo directory in tmp_subDirectoriesInfo)
        {
            Debug.Log("Directory: " + directory.Name);
            var subInfo = new DirectoryInfo(tmp_pathPrefix + directory.Name);
            var subDirectoryInfo = subInfo.GetDirectories();
            foreach (var subDirectory in subDirectoryInfo)
            {
                SongListItem tmp_item = new SongListItem();
                tmp_item.type = directory.Name;
                tmp_item.name = subDirectory.Name;
                tmp_item.songPath = tmp_pathPrefix
                    + directory.Name
                    + "/" + subDirectory.Name;

                Debug.Log(tmp_item.songPath);

                //Only tja exist will be loaded.
                if(File.Exists(tmp_item.songPath + "/"+ tmp_item.name+".tja"))
                {

                    //Create an initial score file when first loaded.
                    string tmp_scoreFilePath = tmp_item.songPath + "/score.txt";
                    try
                    {
                        if (!File.Exists(tmp_scoreFilePath))
                        {
                            File.Create(tmp_scoreFilePath).Close();
                            tmp_item.WriteScore(0);
                        }
                    }

                    catch (Exception ex)
                    {
                        Debug.Log(ex.ToString());
                    }

                    //Add it to list
                    songList.Add(tmp_item);
                    if (!songTypeList.Contains(tmp_item.type))
                    {
                        songTypeList.Add(tmp_item.type);
                    }
                }
            }

            StartCoroutine("LoadAllAudio");
        }
    }

    public IEnumerator LoadAllAudio()
    {
        foreach (SongListItem item in songList)
        {
            StartCoroutine("LoadAudioFile", item);
            yield return null;
        }

        if (callBackMessages != null && callBackMessages.Length > 0) {

            foreach(TargetMessagePair pair in callBackMessages)
            {
                pair.Target.SendMessage(pair.MessageToSend);
            }
            SendMessage("ChangeState", LevelStateManager.LevelState.SongSelection);

        }

        Time.timeScale = 1.0f;
    }

    public IEnumerator LoadAudioFile(SongListItem songListItem)
    {
        string tmp_extention = "";

        if (File.Exists(songListItem.songPath + "/" + songListItem.name + ".ogg"))
        {
            tmp_extention = ".ogg";
        }
        else if (File.Exists(songListItem.songPath + "/" + songListItem.name + ".wav"))
        {
            tmp_extention = ".wav";
        }
        else if (File.Exists(songListItem.songPath + "/" + songListItem.name + ".mp3"))
        {
            tmp_extention = ".mp3";
        }
        else
        {
            Debug.LogError("No audio file, path: " + songListItem.songPath + "/" + songListItem.name );
            yield return null;
        }

        _loadingAudioName = songListItem.name;
        _loadedAudioNumber = songList.IndexOf(songListItem);
        WWW wwwRequest = new WWW("file://" + songListItem.songPath + "/" + songListItem.name + tmp_extention);

        AudioClip tmp_clip = wwwRequest.GetAudioClip();
        while (tmp_clip.loadState != AudioDataLoadState.Loaded)
        {
            //Debug.Log(tmp_clip.loadState);
            yield return wwwRequest;
        }
        tmp_clip.name = songListItem.name;

        songListItem.clip = tmp_clip;
        //loadedClips[index] = tmp_clip;
    }

    public string GetLoadingSongName()
    {
        return _loadingAudioName;
    }

    public string GetLoadingProgressString()
    {
        return String.Format("({0}/{1})", _loadedAudioNumber, songList.Count);
    }
}
