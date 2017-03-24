//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.PoseTracker;
using Valve.VR;

namespace HTC.UnityPlugin.Vive
{
    public interface INewPoseListener
    {
        void BeforeNewPoses();
        void OnNewPoses();
        void AfterNewPoses();
    }
    
    /// <summary>
    /// To manage all NewPoseListeners
    /// </summary>
    public static partial class VivePose
    {
        private static bool hasFocus;

        private static readonly Pose[] poses = new Pose[OpenVR.k_unMaxTrackedDeviceCount];
        private static TrackedDevicePose_t[] rawPoses;

        private static IndexedSet<INewPoseListener> listeners = new IndexedSet<INewPoseListener>();

        static VivePose()
        {
            var system = OpenVR.System;
            if (system != null)
            {
                OnInputFocus(!system.IsInputFocusCapturedByAnotherProcess());
            }
            else
            {
                OnInputFocus(true);
            }

            if (SteamVR_Render.instance != null)
            {
                OnNewPoses(SteamVR_Render.instance.poses);
            }
            else
            {
                OnNewPoses(new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount]);
            }

            SteamVR_Events.InputFocusAction(OnInputFocus).Enable(true);
            SteamVR_Events.NewPosesAction(OnNewPoses).Enable(true);
        }

        public static bool AddNewPosesListener(INewPoseListener listener)
        {
            return listeners.AddUnique(listener);
        }

        public static bool RemoveNewPosesListener(INewPoseListener listener)
        {
            return listeners.Remove(listener);
        }

        private static void OnInputFocus(bool arg)
        {
            hasFocus = arg;
        }

        private static void OnNewPoses(TrackedDevicePose_t[] arg)
        {
            var tempListeners = ListPool<INewPoseListener>.Get();
            tempListeners.AddRange(listeners);

            for (int i = tempListeners.Count - 1; i >= 0; --i)
            {
                tempListeners[i].BeforeNewPoses();
            }

            rawPoses = arg;
            for (int i = rawPoses.Length - 1; i >= 0; --i)
            {
                if (!rawPoses[i].bDeviceIsConnected || !rawPoses[i].bPoseIsValid) { continue; }
                poses[i] = new Pose(rawPoses[i].mDeviceToAbsoluteTracking);
            }

            for (int i = tempListeners.Count - 1; i >= 0; --i)
            {
                tempListeners[i].OnNewPoses();
            }

            for (int i = tempListeners.Count - 1; i >= 0; --i)
            {
                tempListeners[i].AfterNewPoses();
            }

            ListPool<INewPoseListener>.Release(tempListeners);
        }
    }
}