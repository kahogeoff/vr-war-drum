//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using System;
using UnityEngine;
using Valve.VR;

namespace HTC.UnityPlugin.Vive
{
    /// <summary>
    /// To provide static APIs to retrieve controller's button status
    /// </summary>
    [DisallowMultipleComponent]
    public partial class ViveInput : MonoBehaviour
    {
        /// <summary>
        /// Returns true while the button on the controller identified by role is held down
        /// </summary>
        public static bool GetPress(HandRole role, ControllerButton button)
        {
            return GetState(role).GetPress(button);
        }

        /// <summary>
        /// Returns true during the frame the user pressed down the button on the controller identified by role
        /// </summary>
        public static bool GetPressDown(HandRole role, ControllerButton button)
        {
            return GetState(role).GetPressDown(button);
        }

        /// <summary>
        /// Returns true during the frame the user releases the button on the controller identified by role
        /// </summary>
        public static bool GetPressUp(HandRole role, ControllerButton button)
        {
            return GetState(role).GetPressUp(button);
        }

        /// <summary>
        /// Returns time of the last frame that user pressed down the button on the controller identified by role
        /// </summary>
        public static float LastPressDownTime(HandRole role, ControllerButton button)
        {
            return GetState(role).LastPressDownTime(button);
        }

        /// <summary>
        /// Return amount of clicks in a row for the button on the controller identified by role
        /// Set ViveInput.clickInterval to configure click interval
        /// </summary>
        public static int ClickCount(HandRole role, ControllerButton button)
        {
            return GetState(role).ClickCount(button);
        }

        /// <summary>
        /// Returns raw analog value of the trigger button on the controller identified by role
        /// </summary>
        public static float GetTriggerValue(HandRole role, bool usePrevState = false)
        {
            return GetState(role).GetTriggerValue(usePrevState);
        }

        /// <summary>
        /// Returns raw analog value of the touch pad  on the controller identified by role
        /// </summary>
        public static Vector2 GetPadAxis(HandRole role, bool usePrevState = false)
        {
            return GetState(role).GetAxis(usePrevState);
        }

        /// <summary>
        /// Returns raw analog value of the touch pad on the controller identified by role if pressed,
        /// otherwise, returns Vector2.zero
        /// </summary>
        public static Vector2 GetPadPressAxis(HandRole role)
        {
            var handState = GetState(role);
            return handState.GetPress(ControllerButton.Pad) ? handState.GetAxis() : Vector2.zero;
        }

        /// <summary>
        /// Returns raw analog value of the touch pad on the controller identified by role if touched,
        /// otherwise, returns Vector2.zero
        /// </summary>
        public static Vector2 GetPadTouchAxis(HandRole role)
        {
            var handState = GetState(role);
            return handState.GetPress(ControllerButton.PadTouch) ? handState.GetAxis() : Vector2.zero;
        }

        public static Vector2 GetPadPressVector(HandRole role)
        {
            return GetState(role).GetPadPressVector();
        }

        public static Vector2 GetPadTouchVector(HandRole role)
        {
            return GetState(role).GetPadTouchVector();
        }

        public static Vector2 GetPadPressDelta(HandRole role)
        {
            var handState = GetState(role);
            if (handState.GetPress(ControllerButton.Pad) && !handState.GetPressDown(ControllerButton.Pad))
            {
                return handState.GetAxis() - handState.GetAxis(true);
            }
            return Vector2.zero;
        }

        public static Vector2 GetPadTouchDelta(HandRole role)
        {
            var handState = GetState(role);
            if (handState.GetPress(ControllerButton.PadTouch) && !handState.GetPressDown(ControllerButton.PadTouch))
            {
                return handState.GetAxis() - handState.GetAxis(true);
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Add press handler for the button on the controller identified by role
        /// </summary>
        public static void AddPress(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).AddListener(button, callback, ButtonEventType.Press);
        }

        /// <summary>
        /// Add press down handler for the button on the controller identified by role
        /// </summary>
        public static void AddPressDown(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).AddListener(button, callback, ButtonEventType.Down);
        }

        /// <summary>
        /// Add press up handler for the button on the controller identified by role
        /// </summary>
        public static void AddPressUp(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).AddListener(button, callback, ButtonEventType.Up);
        }

        /// <summary>
        /// Add click handler for the button on the controller identified by role
        /// Use ViveInput.ClickCount to get click count
        /// </summary>
        public static void AddClick(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).AddListener(button, callback, ButtonEventType.Click);
        }

        /// <summary>
        /// Remove press handler for the button on the controller identified by role
        /// </summary>
        public static void RemovePress(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).RemoveListener(button, callback, ButtonEventType.Press);
        }

        /// <summary>
        /// Remove press down handler for the button on the controller identified by role
        /// </summary>
        public static void RemovePressDown(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).RemoveListener(button, callback, ButtonEventType.Down);
        }

        /// <summary>
        /// Remove press up handler for the button on the controller identified by role
        /// </summary>
        public static void RemovePressUp(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).RemoveListener(button, callback, ButtonEventType.Up);
        }

        /// <summary>
        /// Remove click handler for the button on the controller identified by role
        /// </summary>
        public static void RemoveClick(HandRole role, ControllerButton button, Action callback)
        {
            GetState(role).RemoveListener(button, callback, ButtonEventType.Click);
        }

        /// <summary>
        /// Trigger vibration of the controller identified by role
        /// </summary>
        public static void TriggerHapticPulse(HandRole role, ushort durationMicroSec = 500)
        {
            var system = OpenVR.System;
            if (system != null)
            {
                system.TriggerHapticPulse(ViveRole.GetDeviceIndex(role), (uint)EVRButtonId.k_EButton_SteamVR_Touchpad - (uint)EVRButtonId.k_EButton_Axis0, (char)durationMicroSec);
            }
        }

        public static VRControllerState_t GetCurrentRawControllerState(HandRole role)
        {
            return GetState(role).GetCurrentRawState();
        }

        public static VRControllerState_t GetPreviousRawControllerState(HandRole role)
        {
            return GetState(role).GetPreviousRawState();
        }
    }
}