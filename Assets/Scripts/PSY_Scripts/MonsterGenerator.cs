using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class MonsterGenerator : MonoBehaviour
{

    public AudioSource audio;
    public GameObject[] monsters;
    public Transform[] spawnsites;
    public Transform spawnsite;
    public Transform hitpoint;
    public float speed = 100.0f;
    //public string songname;
    //public string songpath;

    private string pattern1 = @"";
    private string pattern2 = @"#[A-Z]*";
    private string pattern3 = @"\d*,";
    public int play_index = 0;
    private float flyingtime;

    public struct Rhythm
    {
        public int id;
        public float time;
        public bool isGenerated;
    }
    public List<Rhythm> rhythms;

    // Use this for initialization
    void Start()
    {
        rhythms = new List<Rhythm>();
        Readtja();
        foreach (Rhythm rhythm in rhythms)
        {
            //Debug.Log ("hit id: " + rhythm.id);
            //Debug.Log ("hit time: " + rhythm.time);
        }

        flyingtime = ((hitpoint.position - spawnsite.position).magnitude) / speed;
        Debug.Log(flyingtime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("LeftTrigger") + Input.GetAxis("RightTrigger") > 1.8f)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            else {
                audio.Pause();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audio.Stop();
            /*foreach (Rhythm rhythm in rhythms) {
				rhythm.isGenerated = false;
			}*/
        }

    }

    void FixedUpdate()
    {
        //Debug.Log (Time.fixedDeltaTime);
        //Debug.Log (audio.time);
        //Debug.Log(AudioSettings.dspTime);
        if (audio.isPlaying)
        {
            //Debug.Log ("audio.time: " + audio.time);
            //Debug.Log ("rhythms[i].time: " + rhythms [play_index].time);
            if (audio.time > rhythms[play_index].time - flyingtime)
            {
                //Debug.Log ("count: " + rhythms.Count);
                //Debug.Log ("id: " + rhythms [play_index].id);
                //Debug.Log("play_index: " + play_index);
                GenerateMonster(rhythms[play_index].id);
                if (rhythms.Count - 1 > play_index)
                    play_index++;
            }
        }
    }

    void GenerateMonster(int id)
    {
        switch (id)
        {
            case 0:
                break;
            case 1:
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    Instantiate(monsters[0], spawnsites[0].position, Quaternion.identity);
                }
                else {
                    Instantiate(monsters[2], spawnsites[2].position, Quaternion.identity);
                }
                break;
            case 2:
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    Instantiate(monsters[1], spawnsites[1].position, Quaternion.identity);
                }
                else {
                    Instantiate(monsters[3], spawnsites[3].position, Quaternion.identity);
                }
                break;
            case 3:
                Instantiate(monsters[0], spawnsites[0].position, Quaternion.identity);
                Instantiate(monsters[2], spawnsites[2].position, Quaternion.identity);
                break;
            case 4:
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
        Debug.Log(audio.clip.length);

        Regex regex1 = new Regex(pattern1);
        Regex regex2 = new Regex(pattern2);
        Regex regex3 = new Regex(pattern3);

        string path = Application.dataPath + "/Resources/Blade of Hope/Blade of Hope.tja";
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
                                    tmp.isGenerated = false;
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
}
