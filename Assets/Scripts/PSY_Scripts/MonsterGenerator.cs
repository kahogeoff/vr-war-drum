using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class MonsterGenerator : MonoBehaviour
{
    public LevelStateManager stateManager;

    public AudioSource audio;
    public GameObject[] monsters;
    public Transform[] spawnsites;
    public Transform spawnsite;
    public Transform hitpoint;
    public float speed = 100.0f;
    static public float finalScore;

    public UIControl _UIControl;

    private string pattern1 = @"";
    private string pattern2 = @"#[A-Z]*";
    private string pattern3 = @"\d*,";
    private int num_of_monster = 0;

    public int play_index = 0;
    public float flyingtime;

    private float generate_time = 0.0f;

    [Serializable]
    public struct Rhythm
    {
        public int id;
        public float time;
    }
    public List<Rhythm> rhythms;

    // Use this for initialization
    void Start()
    {
        num_of_monster = 0;

        _UIControl = GameObject.Find("UIObject").GetComponent<UIControl>();

        play_index = 0;
        flyingtime = ((hitpoint.position - spawnsite.position).magnitude) / speed;
        //Debug.Log(flyingtime);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (stateManager.CurrentState)
            {
                case LevelStateManager.LevelState.SongSelection:
                    SendMessage();
                    break;
            }
            InitializeSong();
            if (!audio.isPlaying)
            {
                if (audio.time == 0.0f)
                {
                    PlaybackStart();
                }
                else {
                    PlaybackPause();
                }
            }
            else {
                PlaybackResume();
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audio.Stop();
        }

    }

    void FixedUpdate()
    {
        if(stateManager.CurrentState == LevelStateManager.LevelState.InGame)
        {
            generate_time = rhythms[play_index].time - flyingtime;
            if (audio.isPlaying)
            {
                Debug.Log(string.Format("Time = {0}:{1}", audio.time, rhythms[play_index].time - flyingtime));
                if (audio.time > generate_time && generate_time > 0)
                {
                    GenerateMonster(rhythms[play_index].id);
                    if (rhythms.Count - 1 > play_index)
                        play_index++;
                }
            }
            else if (audio.time > audio.clip.length - 1)
            {
                SendMessage("GameEnd");
            }
            else
            {
                if (audio.time == 0)
                {
                    if (generate_time < 0)
                    {
                        Debug.Log(string.Format("Time = {0}:{1}", audio.time, rhythms[play_index].time - flyingtime));
                        if (play_index == 0)
                        {
                            GenerateMonster(rhythms[play_index].id);
                            play_index++;
                            Debug.Log("generate monster " + play_index);
                        }
                        else
                        {
                            float difference = rhythms[play_index].time - rhythms[play_index - 1].time;
                            if (difference > Time.fixedDeltaTime)
                            {
                                GenerateMonster(rhythms[play_index].id);
                                play_index++;
                                Debug.Log("generate monster " + play_index);

                            }
                        }
                    }
                }
            }
        }
        //Debug.Log(String.Format("Time = {0}/{1}", audio.time, audio.clip.length));
    }

    void GenerateMonster(int id)
    {
        switch (id)
        {
            case 0:
                break;
            case 1:
                num_of_monster++;
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    Instantiate(monsters[0], spawnsites[0].position, Quaternion.identity);
                }
                else {
                    Instantiate(monsters[2], spawnsites[2].position, Quaternion.identity);
                }
                break;
            case 2:
                num_of_monster++;
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    Instantiate(monsters[1], spawnsites[1].position, Quaternion.identity);
                }
                else {
                    Instantiate(monsters[3], spawnsites[3].position, Quaternion.identity);
                }
                break;
            case 3:
                num_of_monster += 2;
                Instantiate(monsters[0], spawnsites[0].position, Quaternion.identity);
                Instantiate(monsters[2], spawnsites[2].position, Quaternion.identity);
                break;
            case 4:
                num_of_monster += 2;
                Instantiate(monsters[1], spawnsites[3].position, Quaternion.identity);
                Instantiate(monsters[3], spawnsites[1].position, Quaternion.identity);
                break;
            case 5:
                //Instantiate (monster5, transform.position, Quaternion.identity);
                break;
            case 6:
                //Instantiate (monster6, transform.position, Quaternion.identity);
                break;
            case 7:
                //Instantiate (monster7, transform.position, Quaternion.identity);
                break;
            case 8:
                //Instantiate (monster8, transform.position, Quaternion.identity);
                break;
            case 9:
                //Instantiate (monster9, transform.position, Quaternion.identity);
                break;
            default:
                Debug.Log("default generate monster");
                break;
        }
    }

    void Readtja()
    {
        //Debug.Log(audio.clip.length);

        Regex regex1 = new Regex(pattern1);
        Regex regex2 = new Regex(pattern2);
        Regex regex3 = new Regex(pattern3);

        string path = Application.dataPath + "/Resources/"+_UIControl.songFullList[_UIControl.songIdx] + ".tja";
        //string path = Application.dataPath + "/Resources/Blade of Hope/Blade of Hope.tja";

        Debug.Log(path);

        if (!File.Exists(path))
        {
            Debug.Log("No existing file.");
        }
        else {
            string line;
            float bpm = 0.0f;
            float crotchet = 0.0f;
            float offset = 0.0f;
            int bar = 0;
            float t = 0.0f;   // time for beats

            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            while ((line = file.ReadLine()) != null)
            {
                Match m1 = regex1.Match(line);
                Match m2 = regex2.Match(line);

                if (m2.Success)
                {
                    if (line == "#START")
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            Match m3 = regex3.Match(line);
                            if (m3.Success)
                            {
                                ++bar;
                                string hits = Regex.Replace(line, ",", "");
                                for (int i = 0; i < hits.Length; i++)
                                {
                                    //Debug.Log (hits[i].GetType());
                                    Rhythm tmp;
                                    tmp.id = (int)Char.GetNumericValue(hits[i]);
                                    tmp.time = t;
                                    rhythms.Add(tmp);
                                    t += crotchet * 4 / hits.Length;
                                }
                            }
                            else {
                                if (line == "#END")
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (line == "#END")
                    {
                        break;
                    }
                }
                else {
                    string[] items = line.Split(':');
                    if (items[0] == "BPM")
                    {
                        bpm = float.Parse(items[1]);
                        crotchet = 60 / bpm;
                    }
                    if (items[0] == "OFFSET")
                    {
                        offset = float.Parse(items[1]);
                        t -= offset;
                    }
                }
            }
            //line = file.ReadToEnd ();
            //Debug.Log (line);
        }
    }

    void InitializeSong()
    {
        rhythms = new List<Rhythm>();
        Readtja();
        play_index = 0;
        generate_time = rhythms[play_index].time - flyingtime;
        audio.clip = Resources.Load<AudioClip>(_UIControl.songFullList[_UIControl.songIdx]);
    }
   
    void PlaybackStart()
    {
        StartCoroutine("OnPlaybackStart");
    }

    void PlaybackPause()
    {
        audio.Pause();
        Time.timeScale = 0.0f;
    }

    void PlaybackResume()
    {
        audio.UnPause();
        Time.timeScale = 1.0f;
    }

    void PlaybackEnd()
    {
        //Is end
        finalScore = (float)CalculateScore.score / (num_of_monster) * 100;
        play_index = 0;
        //_UIControl.SelectBoardAppear();
        Debug.Log("Score: " + finalScore + "%");
        Debug.Log("Max combo: " + CalculateScore.maxcombo);
        Debug.Log("-------------------END------------------");

    }

    IEnumerator OnPlaybackStart()
    {
        InitializeSong();
        yield return new WaitForSeconds(4.0f);
        audio.time = 0.0f;
        audio.Play();
    }

}
