//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.PoseTracker;
using UnityEngine;
using Valve.VR;

namespace HTC.UnityPlugin.Vive
{
    /// <summary>
    /// To provide static APIs to retrieve devices' tracking status
    /// </summary>
    public static partial class VivePose
    {
        /// <summary>
        /// Returns true if input focus captured by current process
        /// Usually the process losses focus when player switch to deshboard by clicking Steam button
        /// </summary>
        public static bool HasFocus() { return hasFocus; }

        /// <summary>
        /// Returns true if the process has focus and the device identified by role is connected / has tracking
        /// </summary>
        public static bool IsValid(HandRole role) { return IsValid(role.ToDeviceRole()); }

        /// <summary>
        /// Returns true if the process has focus and the device identified by role is connected / has tracking
        /// </summary>
        public static bool IsValid(DeviceRole role)
        {
            var index = ViveRole.GetDeviceIndex(role);
            return index < rawPoses.Length && rawPoses[index].bDeviceIsConnected && rawPoses[index].bPoseIsValid && hasFocus;
        }

        /// <summary>
        /// Returns true if the device identified by role is connected.
        /// </summary>
        public static bool IsConnected(HandRole role) { return IsConnected(role.ToDeviceRole()); }

        /// <summary>
        /// Returns true if the device identified by role is connected.
        /// </summary>
        public static bool IsConnected(DeviceRole role)
        {
            var index = ViveRole.GetDeviceIndex(role);
            return index < rawPoses.Length && rawPoses[index].bDeviceIsConnected;
        }

        /// <summary>
        /// Returns true if tracking data of the device identified by role has valid value.
        /// </summary>
        public static bool HasTracking(HandRole role) { return HasTracking(role.ToDeviceRole()); }

        /// <summary>
        /// Returns true if tracking data of the device identified by role has valid value.
        /// </summary>
        public static bool HasTracking(DeviceRole role)
        {
            var index = ViveRole.GetDeviceIndex(role);
            return index < rawPoses.Length && rawPoses[index].bPoseIsValid;
        }

        public static bool IsOutOfRange(HandRole role) { return IsOutOfRange(role.ToDeviceRole()); }

        public static bool IsOutOfRange(DeviceRole role)
        {
            var index = ViveRole.GetDeviceIndex(role);
            return index < rawPoses.Length && (rawPoses[index].eTrackingResult == ETrackingResult.Running_OutOfRange || rawPoses[index].eTrackingResult == ETrackingResult.Calibrating_OutOfRange);
        }

        public static bool IsCalibrating(HandRole role) { return IsCalibrating(role.ToDeviceRole()); }

        public static bool IsCalibrating(DeviceRole role)
        {
            var index = ViveRole.GetDeviceIndex(role);
            return index < rawPoses.Length && (rawPoses[index].eTrackingResult == ETrackingResult.Calibrating_InProgress || rawPoses[index].eTrackingResult == ETrackingResult.Calibrating_OutOfRange);
        }

        public static bool IsUninitialized(HandRole role) { return IsUninitialized(role.ToDeviceRole()); }

        public static bool IsUninitialized(DeviceRole role)
        {
            var index = ViveRole.GetDeviceIndex(role);
            return index < rawPoses.Length && rawPoses[index].eTrackingResult == ETrackingResult.Uninitialized;
        }

        public static Vector3 GetVelocity(HandRole role, Transform origin = null) { return GetVelocity(role.ToDeviceRole(), origin); }

        public static Vector3 GetVelocity(DeviceRole role, Transform origin = null)
        {
            var index = ViveRole.GetDeviceIndex(role);
            var rawValue = Vector3.zero;

            if (index < rawPoses.Length)
            {
                rawValue = new Vector3(rawPoses[index].vVelocity.v0, rawPoses[index].vVelocity.v1, -rawPoses[index].vVelocity.v2);
            }

            return origin == null ? rawValue : origin.TransformVector(rawValue);
        }

        public static Vector3 GetAngularVelocity(HandRole role, Transform origin = null) { return GetAngularVelocity(role.ToDeviceRole(), origin); }

        public static Vector3 GetAngularVelocity(DeviceRole role, Transform origin = null)
        {
            var index = ViveRole.GetDeviceIndex(role);
            var rawValue = Vector3.zero;

            if (index < rawPoses.Length)
            {
                rawValue = new Vector3(-rawPoses[index].vAngularVelocity.v0, -rawPoses[index].vAngularVelocity.v1, rawPoses[index].vAngularVelocity.v2);
            }

            return origin == null ? rawValue : origin.TransformVector(rawValue);
        }

        /// <summary>
        /// Returns tracking pose of the device identified by role
        /// </summary>
        public static Pose GetPose(HandRole role, Transform origin = null) { return GetPose(role.ToDeviceRole(), origin); }

        /// <summary>
        /// Returns tracking pose of the device identified by role
        /// </summary>
        public static Pose GetPose(DeviceRole role, Transform origin = null)
        {
            var index = ViveRole.GetDeviceIndex(role);
            var rawPose = new Pose();

            if (index < poses.Length) { rawPose = poses[index]; }

            if (origin != null)
            {
                rawPose = new Pose(origin) * rawPose;
                rawPose.pos.Scale(origin.localScale);
            }

            return rawPose;
        }

        /// <summary>
        /// Set target pose to tracking pose of the device identified by role relative to the origin
        /// </summary>
        public static void SetPose(Transform target, HandRole role, Transform origin = null) { SetPose(target, role.ToDeviceRole(), origin); }

        /// <summary>
        /// Set target pose to tracking pose of the device identified by role relative to the origin
        /// </summary>
        public static void SetPose(Transform target, DeviceRole role, Transform origin = null) { Pose.SetPose(target, GetPose(role), origin); }
    }
}