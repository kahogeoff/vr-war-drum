using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class TouchSensorScript : MonoBehaviour {
	public string TriggerEnterFunctionName = "StartMoving";
    public string TriggerStayFunctionName = "";
    public string TriggerExitFunctionName = "";
    public bool PressToActive = false;

    [SerializeField]
    protected GameObject _targetObject;

    protected List<Collider> _colliderList = new List<Collider>();

    protected void Start()
    {
        if (!_targetObject)
        {
            _targetObject = transform.parent.gameObject;
        }

        if (string.IsNullOrEmpty(TriggerExitFunctionName))
        {
            TriggerExitFunctionName = TriggerEnterFunctionName;
        }
    }

    protected void OnTriggerEnter(Collider c)
	{
		Debug.Log ("Enter the sensor");
        _colliderList.Add(c);

        if (!PressToActive)
        {
            _targetObject.SendMessage(TriggerEnterFunctionName);
        }

    }

    protected void OnTriggerStay(Collider c)
    {
        //Debug.Log("Stay on sensor");
        if (PressToActive && Input.GetButtonDown("Interact"))
        {
            _targetObject.SendMessage(TriggerEnterFunctionName);
        }else if (!PressToActive && !string.IsNullOrEmpty(TriggerStayFunctionName))
        {
            _targetObject.SendMessage(TriggerStayFunctionName);
        }
    }

    protected void OnTriggerExit(Collider c)
	{
		//Debug.Log ("Exit the sensor");
        _colliderList.Remove(c);
        if (_colliderList.Count < 1)
        {
            _targetObject.SendMessage (TriggerExitFunctionName);
        }
    }
}
