using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {

    [SerializeField]
    private SteamVR_TrackedObject leftController;
    [SerializeField]
    private SteamVR_TrackedObject rightController;

    private SteamVR_Controller.Device leftDevice;
    private SteamVR_Controller.Device rightDevice;

    [SerializeField]
    private bool _StopTrackingController = false;

    // Use this for initialization
    void Start() {
        leftDevice = SteamVR_Controller.Input((int)leftController.index);
        rightDevice = SteamVR_Controller.Input((int)rightController.index);
    }

    void FixedUpdate()
    {
        if (!_StopTrackingController)
        {
            if (leftDevice == null)
                leftDevice = SteamVR_Controller.Input((int)leftController.index);

            if (rightDevice == null)
                rightDevice = SteamVR_Controller.Input((int)rightController.index);
        }
    }

    public bool CheckAnyPressDown(Valve.VR.EVRButtonId keycode)
    {
        if (!CheckControllerValid())
        {
            return false;
        }
        bool result = (leftDevice.GetPressDown(keycode) || rightDevice.GetPressDown(keycode));
        return result;
    }

    public bool CheckBothPressDown(Valve.VR.EVRButtonId keycode)
    {
        if (!CheckControllerValid())
        {
            return false;
        }
        bool result = (leftDevice.GetPressDown(keycode) && rightDevice.GetPressDown(keycode));
        return (result && CheckControllerValid());
    }

    public bool CheckControllerValid()
    {
        if(leftDevice == null || rightDevice == null)
        {
            return false;
        }
        return true;
    }
}
