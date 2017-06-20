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
    public SongListLoader _SongListLoader;
    public int play_index = 0;
    public float flyingtime;

    private string pattern1 = @"";
    private string pattern2 = @"#[A-Z]*";
    private string pattern3 = @"\d*,";
    private int num_of_monster = 0;
    private float generate_time = 0.0f;
    private float game_time = -4.0f;
    private bool continuously_beating_end = false;
    private bool continuous_s = false;
    private float continuous_e = 0.0f;

    [Serializable]
    public struct Rhythm
    {
        public int id;
        public float time;
    }
    public List<Rhythm> rhythms = new List<Rhythm>();

    // Use this for initialization
    void Start()
    {
        num_of_monster = 0;

        _UIControl = GameObject.Find("UIObject").GetComponent<UIControl>();
        _SongListLoader = GameObject.Find("UIObject").GetComponent<SongListLoader>();

        play_index = 0;
        flyingtime = ((hitpoint.position - spawnsite.position).magnitude) / speed;
        //Debug.Log(flyingtime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //audio.Stop();
        }

    }

    void FixedUpdate()
    {
        if(stateManager.CurrentState == LevelStateManager.LevelState.SongSelection)
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }

        if(stateManager.CurrentState == LevelStateManager.LevelState.InGame)
        {
            if (rhythms.Count < 1 || rhythms == null)
            {
                //rhythms = new List<Rhythm>();
                //Readtja();
                return;
            }
            generate_time = rhythms[play_index].time - flyingtime;
            if (audio.isPlaying)
            {
                //Debug.Log(string.Format("Time = {0}:{1}", audio.time, rhythms[play_index].time - flyingtime));
                if (audio.time > generate_time && generate_time > 0)
                {
                    GenerateMonster(rhythms[play_index].id, play_index);
                    if (rhythms.Count - 1 > play_index)
                        play_index++;
                }
                if (audio.time > continuous_e && continuous_e != 0.0f)
                {
                    monster.continuously_beating = true;
                    continuous_e = 0.0f;
                }

                Debug.Log(string.Format("End to continuous: {0} && {1}", audio.time, continuous_e));
            }
            else
            {
                //  Game starts but music not playing
                if (audio.time == 0)
                {
                    if (generate_time < 0)
                    {

                        if (game_time < generate_time)
                        {
                            //Do nothing
                        }
                        else
                        {
                            Debug.Log(string.Format("Time = {0}:{1}", audio.time, rhythms[play_index].time - flyingtime));

                            if (play_index == 0)
                            {
                                GenerateMonster(rhythms[play_index].id, play_index);
                                play_index++;
                                Debug.Log("generate monster " + play_index);
                            }
                            else
                            {
                                float difference = rhythms[play_index].time - rhythms[play_index - 1].time;
                                if (difference > Time.fixedDeltaTime)
                                {
                                    GenerateMonster(rhythms[play_index].id, play_index);
                                    play_index++;
                                    Debug.Log("generate monster " + play_index);

                                }
                            }
                        }
                        
                        game_time += Time.fixedDeltaTime;
                    }
                }
            }
            
            if (audio.time >= audio.clip.length - 1)
            {
                SendMessage("GameEnd");
            }
        }
        //Debug.Log(String.Format("Time = {0}/{1}", audio.time, audio.clip.length));
    }

    void GenerateMonster(int id, int play_index)
    {
        if (continuously_beating_end)
        {
            Debug.Log(string.Format("Time = {0}:{1}", audio.time, rhythms[play_index].time + " " + rhythms[play_index].id));
            continuous_e = rhythms[play_index].time;
            continuously_beating_end = false;
        }
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
                if (!continuous_s)
                {
                    continuous_s = true;
                    Instantiate(monsters[4], spawnsites[4].position, Quaternion.identity);
                }
                break;
            case 6:
                if (!continuous_s)
                {
                    continuous_s = true;
                    Instantiate(monsters[4], spawnsites[4].position, Quaternion.identity);
                }
                break;
            case 7:
                if (!continuous_s)
                {
                    continuous_s = true;
                    Instantiate(monsters[4], spawnsites[4].position, Quaternion.identity);
                }
                break;
            case 8:
                // End of continuous hitting
                //Instantiate (monster8, transform.position, Quaternion.identity);
                //Debug.Log("----------meet 8------------");
                continuous_s = false;
                continuously_beating_end = true;
                //Debug.Log("888888888888888888888");
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

        string path = /*Application.dataPath + "Assets/Resources/"+*/ _SongListLoader.songList[_UIControl.songIdx].songPath + "/" + _SongListLoader.songList[_UIControl.songIdx].name + ".tja";
        //string path = Application.dataPath + "/Resources/Blade of Hope/Blade of Hope.tja";

        Debug.Log(path);

        if (!File.Exists(path))
        {
            Debug.Log("No existing file.");
        }
        else
        {
            string line;
            float bpm = 0.0f;
            float crotchet = 0.0f;
            float quaver = 0.0f;
            float offset = 0.0f;
            float bar_time = 0.0f;
            float t = 0.0f;   // time for beats

            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            while ((line = file.ReadLine()) != null)
            {
                Match m1 = regex1.Match(line);
                Match m2 = regex2.Match(line);
                string pattern4 = @"#[A-Z]* [0-9]+\.[0-9]+";
                Regex regex4 = new Regex(pattern4);
                string pattern5 = @"#[A-Z]* [0-9]/[0-9]";
                Regex regex5 = new Regex(pattern5);

                if (m2.Success)
                {
                    if (line == "#START")
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            Match m3 = regex3.Match(line);
                            if (m3.Success)
                            {
                                if (line == ",")
                                {
                                    Debug.Log(line);
                                    t += bar_time;
                                }
                                else
                                {
                                    string[] words = line.Split(',');
                                    Debug.Log(words[0]);
                                    //string hits = Regex.Replace(line, ",", "");
                                    for (int i = 0; i < words[0].Length; i++)
                                    {
                                        //Debug.Log (hits[i].GetType());
                                        Rhythm tmp;
                                        tmp.id = (int)Char.GetNumericValue(words[0][i]);
                                        tmp.time = t;
                                        rhythms.Add(tmp);
                                        t += bar_time / words[0].Length;
                                    }
                                }
                            }
                            else {
                                if (line == "#END")
                                {
                                    break;
                                }

                                Match m4 = regex4.Match(line);
                                if (m4.Success)
                                {
                                    string[] words = line.Split(' ');
                                    if (words[0] == "#DELAY")
                                    {
                                        t += float.Parse(words[1]);
                                    }
                                    else if (words[0] == "#BPMCHANGE")
                                    {
                                        bpm = float.Parse(words[1]);
                                        crotchet = 60 / bpm;
                                        quaver = crotchet / 2;
                                        bar_time = crotchet * 4;
                                    }
                                }

                                Match m5 = regex5.Match(line);
                                if (m5.Success)
                                {
                                    string[] words = line.Split(' ');
                                    string[] fractions = words[1].Split('/');
                                    float measure = float.Parse(fractions[0]) / float.Parse(fractions[1]);
                                    bar_time *= measure;
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

                        quaver = crotchet / 2;
                        bar_time = crotchet * 4;
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
        game_time = -flyingtime;
        //Debug.Log(rhythms.Count);
        //generate_time = rhythms[play_index].time - flyingtime;
        audio.clip = _SongListLoader.songList[_UIControl.songIdx].clip;//Resources.Load<AudioClip>(songFullList[_UIControl.songIdx]);
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
        audio.Stop();
        //Is end
        finalScore = (float)CalculateScore.score / (num_of_monster) * 100;
        play_index = 0;
        game_time = 0;

        //_UIControl.SelectBoardAppear();
        //Debug.Log("Score: " + finalScore + "%");
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
