using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumScript : MonoBehaviour {
    public enum DrumType
    {
        NotADrum ,Red , Blue
    }
    public enum DrumPosition
    {
        None, Left, Right
    }
    public MonsterGenerator Generator;

    public Transform Gunpoint;
    public GameObject Bullet;
    public GameObject Gun;
    public DrumType Type = DrumType.Red;
    public float AcceptableHittingForce = 0.1f;

    private List<GameObject> _colliedObjects = new List<GameObject>();
    public bool _canFire = false;

    public bool selectionMode = true;
    private LevelStateManager.LevelState currentState;
    public DrumPosition Side = DrumPosition.Left;
    public UIControl _UIControl;
    public Animator _selfAnim;
    public string AnimationName;

    // Use this for initialization
    void Start () {
        Generator = GameObject.Find("[LevelManager]").GetComponent<MonsterGenerator>();
        _UIControl = GameObject.Find("UIObject").GetComponent<UIControl>();
        _selfAnim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ShootBullet()
    {
        if(Generator.audio.isPlaying)
        {
            //Debug.Log(string.Format("Difference[{0}]= {1}", Generator.play_index, 
              //  Generator.difficulties[MonsterGenerator.choosed_difficulty].rhythms[Generator.play_index].time - Generator.audio.time));
        }

        if (Gun)
        {
            Gun.SendMessage("FireTheGun", Gunpoint.position);
        }
        else
        {
            Instantiate<GameObject>(Bullet, Gunpoint.position, Gunpoint.rotation);
        }

        /*
        if (selectionMode)
        {
            if(Type == DrumType.Red)
            {
                if (Side == DrumPosition.Left)
                {
                    _UIControl.ChangeSongLeft();
                }
                else if(Side == DrumPosition.Right)
                {
                    _UIControl.ChangeSongRight();
                }
            }else if (Type == DrumType.Blue)
            {
                if (Side == DrumPosition.Left)
                {
                    _UIControl.ChangeSongTypeLeft();
                }else if (Side == DrumPosition.Right)
                {
                    _UIControl.ChangeSongTypeRight();
                }
            }
        }
        */
    }

    void EnableFiring()
    {
        _canFire = true;
    }

    void DisableFiring()
    {
        _canFire = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.relativeVelocity.magnitude);
        if (_canFire && !_colliedObjects.Contains(collision.gameObject))
        {
            _colliedObjects.Add(collision.gameObject);
        }

        float tmp_hitForce = collision.relativeVelocity.magnitude;
        //Debug.Log("Force = " + tmp_hitForce);
        if (_canFire && tmp_hitForce > AcceptableHittingForce && _colliedObjects.Count < 2)
        {
            gameObject.SendMessage("ShootBullet");
            _selfAnim.Play(AnimationName, -1, 0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_colliedObjects.Contains(collision.gameObject))
        {
            _colliedObjects.Remove(collision.gameObject);
        }
    }
}
