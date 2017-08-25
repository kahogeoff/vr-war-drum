using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

[Serializable]
public class Rhythm
{
    public int id;
    public float time;
    public float duration;
    public float combo_duration;
}

[Serializable]
public class Difficulty
{
    public string course;
    public int level;
    public List<int> balloons;
    public int score_init;
    public int score_diff;
    public List<Rhythm> rhythms;


    public Difficulty(string course, int level, List<int> balloons, int score_init, int score_diff, List<Rhythm> rhythms)
    {
        this.course = course;
        this.level = level;
        this.balloons = balloons;
        this.score_diff = score_diff;
        this.score_init = score_init;
        this.rhythms = rhythms;
    }

    public Difficulty(string course, int level, int score_init, int score_diff, List<Rhythm> rhythms)
    {
        this.course = course;
        this.level = level;
        this.score_diff = score_diff;
        this.score_init = score_init;
        this.rhythms = rhythms;
    }
}

public class MonsterGenerator : MonoBehaviour
{
    public LevelStateManager stateManager;

    public float demo_start = 0.0f;

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
    public GameObject spawnEffect;
    //public int choosed_difficulty = 3;
    static public int choosed_difficulty = 3;

    private int num_of_monster = 0;
    private float generate_time = 0.0f;
    private float game_time = -4.0f;
    private string[] DIFFICULTY_TYPE
    {
        get { return new string[] { "Easy", "Normal", "Hard", "Oni" }; }
    }

    public Difficulty[] difficulties = new Difficulty[4];

    // Use this for initialization
    void Start()
    {
        num_of_monster = 0;

        if (_UIControl == null)
        {
            _UIControl = GetComponent<UIControl>();
        }

        if (_SongListLoader == null)
        {
            _SongListLoader = GetComponent<SongListLoader>();
        }

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
        switch (stateManager.CurrentState)
        {
            //Song selection stage
            case LevelStateManager.LevelState.SongSelection:

                audio.clip = _SongListLoader.songList[_UIControl.songIdx].clip;
                if (audio.isPlaying)
                {
                    audio.Stop();
                }
                break;

            //In game stage
            case LevelStateManager.LevelState.InGame:

                //If not rhythms (load error) and reach the end of rhythms list
                if (difficulties[choosed_difficulty].rhythms.Count < 1 || difficulties[choosed_difficulty].rhythms == null)
                {
                    Debug.Log("Load error");
                    stateManager.SendMessage("GameEnd");
                    return;
                }

                if (audio.time >= audio.clip.length - 1)
                {
                    stateManager.SendMessage("GameEnd");
                    return;
                }

                if (play_index == difficulties[choosed_difficulty].rhythms.Count)
                {
                    return;
                }

                //Debug.Log("play_index: " + play_index);

                //Initial the generate timer
                generate_time = difficulties[choosed_difficulty].rhythms[play_index].time - flyingtime;
                if (audio.isPlaying)
                {
                    if (audio.time > generate_time && generate_time > 0)
                    {
                        GenerateMonster(difficulties[choosed_difficulty].rhythms[play_index].id, play_index);
                        if (difficulties[choosed_difficulty].rhythms.Count > play_index)
                            play_index++;
                    }
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
                                if (play_index == 0)
                                {
                                    GenerateMonster(difficulties[choosed_difficulty].rhythms[play_index].id, play_index);
                                    play_index++;
                                    //Debug.Log("generate monster " + play_index);
                                }
                                else
                                {
                                    float difference = difficulties[choosed_difficulty].rhythms[play_index].time - difficulties[choosed_difficulty].rhythms[play_index - 1].time;
                                    if (difference > Time.fixedDeltaTime)
                                    {
                                        GenerateMonster(difficulties[choosed_difficulty].rhythms[play_index].id, play_index);
                                        play_index++;
                                        //.Log("generate monster " + play_index);

                                    }
                                }
                            }

                            game_time += Time.fixedDeltaTime;
                        }
                    }
                }

                if (audio.time >= audio.clip.length - 1)
                {
                    stateManager.SendMessage("GameEnd");
                }
                break;
        }
    }

    void GenerateMonster(int id, int play_index)
    {

        GameObject yello_monster;
        GameObject tmp_monster = null;
        GameObject tmp_monster_L = null;
        GameObject tmp_monster_R = null;

        switch (id)
        {
            case 0:
                break;
            case 1:
            case 2:
                num_of_monster++;
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    tmp_monster = Instantiate(monsters[id - 1], spawnsites[id - 1].position, Quaternion.identity);
                    Instantiate(spawnEffect, spawnsites[id - 1].position, Quaternion.identity);
                }
                else {
                    tmp_monster = Instantiate(monsters[id + 1], spawnsites[id + 1].position, Quaternion.identity);
                    Instantiate(spawnEffect, spawnsites[id + 1].position, Quaternion.identity);
                }
                tmp_monster.GetComponent<monster>().speed = speed;
                break;

            case 3:
            case 4:
                num_of_monster += 2;
                tmp_monster_L = Instantiate(monsters[id - 3], spawnsites[id - 3].position, Quaternion.identity);
                tmp_monster_R = Instantiate(monsters[id - 1], spawnsites[id - 1].position, Quaternion.identity);
                Instantiate(spawnEffect, spawnsites[id - 3].position, Quaternion.identity);
                Instantiate(spawnEffect, spawnsites[id - 1].position, Quaternion.identity);

                tmp_monster_L.GetComponent<monster>().speed = speed;
                tmp_monster_R.GetComponent<monster>().speed = speed;
                break;

            case 5:
            case 6:
            case 7:
                yello_monster = Instantiate(monsters[4], spawnsites[4].position, Quaternion.identity);
                Instantiate(spawnEffect, spawnsites[4].position, Quaternion.identity);
                yello_monster.GetComponent<monster>().speed = speed;
                //float tmp = difficulties[choosed_difficulty].rhythms[play_index].duration + flyingtime;
                //Debug.Log("Destroy yello: " + tmp);
                Destroy(yello_monster, difficulties[choosed_difficulty].rhythms[play_index].combo_duration + flyingtime);
                break;

            case 8:
            case 9:
                //Do nothing
                break;
            default:
                Debug.Log("default generate monster");
                break;
        }
    }

    void Readtja()
    {
        //Debug.Log(audio.clip.length);
        string path = _SongListLoader.songList[_UIControl.songIdx].songPath + "/" + _SongListLoader.songList[_UIControl.songIdx].name + ".tja";

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
            int level = 0;
            int score_init = 0;
            int score_diff = 0;
            string course = "Easy";
            int d = 0;
            List<Rhythm> rhythms = new List<Rhythm>();

            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            while ((line = file.ReadLine()) != null)
            {
                string pattern1 = @"";
                string pattern2 = @"#[A-Z]*";

                Regex regex1 = new Regex(pattern1);
                Regex regex2 = new Regex(pattern2);


                string[] items = line.Split(':');
                //Debug.Log(items[0]);
                int i = 0;
                foreach (string item in items)
                {
                    i++;
                }
                switch (items[0])
                {
                    case "BPM":
                        bpm = float.Parse(items[1]);
                        crotchet = 60 / bpm;

                        quaver = crotchet / 2;
                        bar_time = crotchet * 4;
                        break;
                    case "OFFSET":
                        float.TryParse(items[1], out offset);
                        break;
                    case "DEMOSTART":
                        float.TryParse(items[1], out demo_start);
                        break;
                    case "COURSE":
                        {
                            float t = -offset;   // time for beats
                            if (!int.TryParse(items[1], out d))
                            {
                                switch (items[1].ToUpper())
                                {
                                    case "ONI":
                                        d = 3;
                                        break;
                                    case "HARD":
                                        d = 2;
                                        break;
                                    case "NORMAL":
                                        d = 1;
                                        break;
                                    case "EASY":
                                        d = 0;
                                        break;
                                }
                            }

                            course = DIFFICULTY_TYPE[d];

                            while ((line = file.ReadLine()) != null)
                            {
                                Match m2 = regex2.Match(line);

                                if (m2.Success)
                                {
                                    // between #START and #END
                                    if (line == "#START")
                                    {
                                        readFromStartToEnd(ref file, ref line, ref bpm, ref crotchet, ref quaver, ref offset, ref bar_time, ref rhythms, ref t);
                                    }
                                    if (line == "#END")
                                    {
                                        // escape the while starting from reading COURSE
                                        break;
                                    }
                                }
                                else
                                {
                                    items = line.Split(':');
                                    i = 0;
                                    foreach (string item in items)
                                    {
                                        i++;
                                    }
                                    if (i > 1 && items[1] != "")
                                    {
                                        //Debug.Log(items[1]);
                                        switch (items[0])
                                        {
                                            case "LEVEL":
                                                level = int.Parse(items[1]);
                                                break;
                                            case "BALLOON":
                                                break;
                                            case "SCOREINIT":
                                                score_init = int.Parse(items[1]);
                                                break;
                                            case "SCOREDIFF":
                                                score_diff = int.Parse(items[1]);
                                                break;
                                        }
                                    }
                                }
                            }
                            difficulties[d] = new Difficulty(course, level, score_init, score_diff, rhythms);
                            //Debug.Log(d);
                        }
                        break;
                    case "LEVEL":
                        level = int.Parse(items[1]);
                        if (level > 0 && level < 5)
                        {
                            course = "Easy";
                            d = 0;
                        }
                        else if (level == 5 || level == 6)
                        {
                            course = "Normal";
                            d = 1;
                        }
                        else if (level == 7 || level == 8)
                        {
                            course = "Hard";
                            d = 2;
                        }
                        else
                        {
                            course = "Oni";
                            d = 3;
                        }
                        break;
                    case "BALLOON":
                        break;
                    case "SCOREINIT":
                        if (i > 1 && !string.IsNullOrEmpty(items[1]))
                            int.TryParse(items[1], out score_init);
                        break;
                    case "SCOREDIFF":
                        if (i > 1 && !string.IsNullOrEmpty(items[1]))
                            int.TryParse(items[1], out score_diff);
                        break;
                    case "#START":
                        {
                            Debug.Log("level: " + level);
                            Debug.Log("d: " + d);
                            float t = -offset;
                            readFromStartToEnd(ref file, ref line, ref bpm, ref crotchet, ref quaver, ref offset, ref bar_time, ref rhythms, ref t);
                            difficulties[d] = new Difficulty(course, level, score_init, score_diff, rhythms);
                        }
                        break;
                }
            }
            //line = file.ReadToEnd ();
            //Debug.Log (line);
        }
    }

    void readFromStartToEnd(
        ref System.IO.StreamReader file,
        ref string line,
        ref float bpm,
        ref float crotchet,
        ref float quaver,
        ref float offset,
        ref float bar_time,
        ref List<Rhythm> rhythms,
        ref float t
        )
    {
        string pattern3 = @"\d*,";
        string pattern4 = @"#[A-Z]* [0-9]+\.[0-9]+";
        string pattern5 = @"#[A-Z]* [0-9]/[0-9]";

        Regex regex3 = new Regex(pattern3);
        Regex regex4 = new Regex(pattern4);
        Regex regex5 = new Regex(pattern5);

        while ((line = file.ReadLine()) != null)
        {
            Match m3 = regex3.Match(line);
            if (m3.Success)
            {
                if (line == ",")
                {
                    //Debug.Log(line);
                    Rhythm tmp = new Rhythm();
                    tmp.id = 0;
                    tmp.time = t;
                    tmp.duration = bar_time;
                    rhythms.Add(tmp);
                    t += bar_time;
                }
                else
                {
                    string[] words = line.Split(',');
                    //Debug.Log(words[0]);
                    //string hits = Regex.Replace(line, ",", "");
                    for (int i = 0; i < words[0].Length; i++)
                    {
                        //Debug.Log (hits[i].GetType());
                        Rhythm tmp = new Rhythm();
                        tmp.id = (int)Char.GetNumericValue(words[0][i]);
                        tmp.time = t;
                        tmp.duration = bar_time / words[0].Length;
                        rhythms.Add(tmp);
                        t += bar_time / words[0].Length;
                    }
                }
            }
            else
            {
                if (line == "#END")
                {
                    // escape the while starting from reading #START
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
                    if (float.Parse(fractions[1]) == 4)
                    {
                        bar_time = crotchet * float.Parse(fractions[0]);
                    }
                    else
                    {
                        //Debug or do nothing
                        //Debug.Log("One beat: " + float.Parse(fractions[1]));
                    }
                }
            }
        }
    }

    void calComboDuration()
    {
        //Debug.Log("calDuration");
        for (int i = 0; i < difficulties[choosed_difficulty].rhythms.Count; i++)
        {
            if (difficulties[choosed_difficulty].rhythms[i].id == 5 || difficulties[choosed_difficulty].rhythms[i].id == 6 || difficulties[choosed_difficulty].rhythms[i].id == 7)
            {
                for (int j = i + 1; j < difficulties[choosed_difficulty].rhythms.Count; j++)
                {
                    if (difficulties[choosed_difficulty].rhythms[j].id == 8)
                    {
                        //Debug. Log("combo start: " + difficulties[choosed_difficulty].rhythms[i].time);
                        //Debug.Log("combo end: " + difficulties[choosed_difficulty].rhythms[j + 1].time);
                        difficulties[choosed_difficulty].rhythms[i].combo_duration = difficulties[choosed_difficulty].rhythms[j].time + difficulties[choosed_difficulty].rhythms[j].duration - difficulties[choosed_difficulty].rhythms[i].time;
                        //Debug.Log(rhythms[i].duration);
                        i = j;
                        break;
                    }
                }
            }
            //Debug.Log(_rhythms[i].duration);
        }

    }

    void InitializeSong()
    {
        flyingtime = ((hitpoint.position - spawnsite.position).magnitude) / speed;

        Readtja();
        play_index = 0;
        game_time = -flyingtime;

        for (int i = 0; i < 4; i++)
        {
            if (difficulties[i] != null)
            {
                if (difficulties[i].level != 0)
                {
                    Debug.Log("Difficulty: " + i);
                    choosed_difficulty = i;
                    _UIControl.songDifficulty = choosed_difficulty;
                    _UIControl.SetSelectedDifficulty(_UIControl.songDifficulty);
                    break;
                }
            }
        }
        Debug.Log("Number of rhythms: " + difficulties[choosed_difficulty].rhythms.Count);
        calComboDuration();
    }

    void PrepareTheSong()
    {
        InitializeSong();
    }

    void UnloadTheSong()
    {
        difficulties = new Difficulty[4];
    }

    void PlaybackStart()
    {
        StopCoroutine("OnPlaybackStart");
        if (difficulties[_UIControl.songDifficulty] != null)
        {
            if (difficulties[_UIControl.songDifficulty].level != 0)
            {
                stateManager.SendMessage("GameStart");
                StartCoroutine("OnPlaybackStart");
            }
        }
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
        difficulties = new Difficulty[4];

        //_UIControl.SelectBoardAppear();
        //Debug.Log("Score: " + finalScore + "%");
        //Debug.Log("Max combo: " + CalculateScore.maxcombo);
        //Debug.Log("-------------------END------------------");

    }

    IEnumerator OnPlaybackStart()
    {
        //InitializeSong();
        yield return new WaitForSeconds(flyingtime);
        audio.time = 0.0f;
        audio.Play();
    }

}
