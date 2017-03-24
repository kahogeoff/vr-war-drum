//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.PoseTracker;
using HTC.UnityPlugin.Utility;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HTC.UnityPlugin.Vive
{
    [AddComponentMenu("HTC/Vive/Vive Pose Tracker")]
    // Simple component to track Vive devices.
    public class VivePoseTracker : BasePoseTracker, INewPoseListener
    {
        [Serializable]
        public class UnityEventBool : UnityEvent<bool> { }
        [Serializable]
        public class UnityEventUint : UnityEvent<uint> { }

        private bool isValid;

        public Transform origin;
        public DeviceRole role = DeviceRole.Hmd;
        public UnityEventBool onIsValidChanged;

        public bool isPoseValid { get { return isValid; } }

        protected void SetIsValid(bool value, bool forceSet = false)
        {
            if (ChangeProp.Set(ref isValid, value) || forceSet)
            {
                onIsValidChanged.Invoke(value);
            }
        }

        protected virtual void Start()
        {
            SetIsValid(false, true);
        }

        protected virtual void OnEnable()
        {
            VivePose.AddNewPosesListener(this);
        }

        protected virtual void OnDisable()
        {
            VivePose.RemoveNewPosesListener(this);

            SetIsValid(false);
        }

        public virtual void BeforeNewPoses() { }

        public virtual void OnNewPoses()
        {
            TrackPose(VivePose.GetPose(role), origin);

            SetIsValid(VivePose.IsValid(role));
        }

        public virtual void AfterNewPoses() { }
    }
}