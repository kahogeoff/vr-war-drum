//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace HTC.UnityPlugin.Vive
{
    // Data structure for storing buttons status.
    public partial class ViveInput : MonoBehaviour
    {
        private class ControllerState
        {
            public uint index = OpenVR.k_unTrackedDeviceIndexInvalid;
            public readonly HandRole role;

            private int prevFrameCount = -1;
            private VRControllerState_t currentState;
            private VRControllerState_t previousState;

            private List<float> lastDownTimes = new List<float>(CONTROLLER_BUTTON_COUNT);
            private List<int> clickeCount = new List<int>(CONTROLLER_BUTTON_COUNT);

            private List<Action> downListeners = new List<Action>(CONTROLLER_BUTTON_COUNT);
            private List<Action> pressListeners = new List<Action>(CONTROLLER_BUTTON_COUNT);
            private List<Action> upListeners = new List<Action>(CONTROLLER_BUTTON_COUNT);
            private List<Action> clickListeners = new List<Action>(CONTROLLER_BUTTON_COUNT);

            private Vector2 padDownAxis;
            private Vector2 padTouchDownAxis;

            public ControllerState(HandRole role)
            {
                this.role = role;
                for (int i = CONTROLLER_BUTTON_COUNT - 1; i >= 0; --i)
                {
                    lastDownTimes.Add(0f);
                    clickeCount.Add(0);

                    downListeners.Add(null);
                    pressListeners.Add(null);
                    upListeners.Add(null);
                    clickListeners.Add(null);
                }
            }

            public void Update()
            {
                if (Time.frameCount != prevFrameCount)
                {
                    prevFrameCount = Time.frameCount;
                    previousState = currentState;

                    CVRSystem system;
                    index = ViveRole.GetDeviceIndex(role);
                    if (!ViveRole.IsValidIndex(index) || (system = OpenVR.System) == null || !system.GetControllerState(index, ref currentState, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VRControllerState_t))))
                    {
                        currentState = default(VRControllerState_t);
                    }

                    UpdateHairTrigger();

                    if (GetPressDown(ControllerButton.Pad)) { padDownAxis = GetAxis(); }
                    if (GetPressDown(ControllerButton.PadTouch)) { padTouchDownAxis = GetAxis(); }

                    for (int i = 0; i < ViveInput.CONTROLLER_BUTTON_COUNT; ++i)
                    {
                        UpdateClickCount((ControllerButton)i);
                    }

                    for (int i = 0; i < ViveInput.CONTROLLER_BUTTON_COUNT; ++i)
                    {
                        InvokeEvent((ControllerButton)i);
                    }
                }
            }

            private List<Action> GetListenerList(ButtonEventType type)
            {
                switch (type)
                {
                    case ButtonEventType.Down: return downListeners;
                    case ButtonEventType.Press: return pressListeners;
                    case ButtonEventType.Up: return upListeners;
                    case ButtonEventType.Click: return clickListeners;
                    default: return null;
                }
            }

            public void AddListener(ControllerButton button, Action listener, ButtonEventType type = ButtonEventType.Click)
            {
                List<Action> listeners;
                if (listener == null || (listeners = GetListenerList(type)) == null) { return; }
                var index = (int)button;
                if (index >= 0 && index < CONTROLLER_BUTTON_COUNT)
                {
                    if (listeners[index] == null)
                    {
                        listeners[index] = listener;
                    }
                    else
                    {
                        listeners[index] += listener;
                    }
                }
            }

            public void RemoveListener(ControllerButton button, Action listener, ButtonEventType type = ButtonEventType.Click)
            {
                List<Action> listeners;
                if (listener == null || (listeners = GetListenerList(type)) == null) { return; }
                var index = (int)button;
                if (index >= 0 && index < CONTROLLER_BUTTON_COUNT)
                {
                    if (listeners[index] != null)
                    {
                        listeners[index] -= listener;
                    }
                }
            }

            private void UpdateClickCount(ControllerButton button)
            {
                var index = (int)button;
                if (GetPressDown(button))
                {
                    if (Time.time - lastDownTimes[index] < ViveInput.clickInterval)
                    {
                        ++clickeCount[index];
                    }
                    else
                    {
                        clickeCount[index] = 1;
                    }

                    lastDownTimes[index] = Time.time;
                }
            }

            private void InvokeEvent(ControllerButton button)
            {
                var index = (int)button;
                if (GetPress(button))
                {
                    if (GetPressDown(button))
                    {
                        // PressDown event
                        if (downListeners[index] != null) { downListeners[index].Invoke(); }
                    }

                    // Press event
                    if (pressListeners[index] != null) { pressListeners[index].Invoke(); }
                }
                else if (GetPressUp(button))
                {
                    // PressUp event
                    if (upListeners[index] != null) { upListeners[index].Invoke(); }

                    if (Time.time - lastDownTimes[index] < ViveInput.clickInterval)
                    {
                        // Click event
                        if (clickListeners[index] != null) { clickListeners[index].Invoke(); }
                    }
                }
            }

            public bool GetPress(ControllerButton button)
            {
                Update();
                switch (button)
                {
                    case ControllerButton.Trigger: return GetPress(EVRButtonId.k_EButton_SteamVR_Trigger);
                    case ControllerButton.FullTrigger: return currentState.rAxis1.x >= 1f;
                    case ControllerButton.HairTrigger: return GetHairTrigger();
                    case ControllerButton.Pad: return GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
                    case ControllerButton.PadTouch: return GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad);
                    case ControllerButton.Grip: return GetPress(EVRButtonId.k_EButton_Grip);
                    case ControllerButton.Menu: return GetPress(EVRButtonId.k_EButton_ApplicationMenu);
                    default: return false;
                }
            }

            public bool GetPressDown(ControllerButton button)
            {
                Update();
                switch (button)
                {
                    case ControllerButton.Trigger: return GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger);
                    case ControllerButton.FullTrigger: return currentState.rAxis1.x >= 1f && previousState.rAxis1.x < 1f;
                    case ControllerButton.HairTrigger: return GetHairTriggerDown();
                    case ControllerButton.Pad: return GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad);
                    case ControllerButton.PadTouch: return GetTouchDown(EVRButtonId.k_EButton_SteamVR_Touchpad);
                    case ControllerButton.Grip: return GetPressDown(EVRButtonId.k_EButton_Grip);
                    case ControllerButton.Menu: return GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);
                    default: return false;
                }
            }

            public bool GetPressUp(ControllerButton button)
            {
                Update();
                switch (button)
                {
                    case ControllerButton.Trigger: return GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger);
                    case ControllerButton.FullTrigger: return currentState.rAxis1.x < 1f && previousState.rAxis1.x >= 1f;
                    case ControllerButton.HairTrigger: return GetHairTriggerUp();
                    case ControllerButton.Pad: return GetPressUp(EVRButtonId.k_EButton_SteamVR_Touchpad);
                    case ControllerButton.PadTouch: return GetTouchUp(EVRButtonId.k_EButton_SteamVR_Touchpad);
                    case ControllerButton.Grip: return GetPressUp(EVRButtonId.k_EButton_Grip);
                    case ControllerButton.Menu: return GetPressUp(EVRButtonId.k_EButton_ApplicationMenu);
                    default: return false;
                }
            }

            public float LastPressDownTime(ControllerButton button)
            {
                var index = (int)button;
                if (index >= 0 && index < CONTROLLER_BUTTON_COUNT)
                {
                    Update();
                    return lastDownTimes[index];
                }
                else
                {
                    return 0f;
                }
            }

            public int ClickCount(ControllerButton button)
            {
                var index = (int)button;
                if (index >= 0 && index < CONTROLLER_BUTTON_COUNT)
                {
                    Update();
                    return clickeCount[index];
                }
                else
                {
                    return 0;
                }
            }

            public bool GetPress(ulong buttonMask) { Update(); return (currentState.ulButtonPressed & buttonMask) != 0; }
            public bool GetPressDown(ulong buttonMask) { Update(); return (currentState.ulButtonPressed & buttonMask) != 0 && (previousState.ulButtonPressed & buttonMask) == 0; }
            public bool GetPressUp(ulong buttonMask) { Update(); return (currentState.ulButtonPressed & buttonMask) == 0 && (previousState.ulButtonPressed & buttonMask) != 0; }

            public bool GetPress(EVRButtonId buttonId) { return GetPress(1ul << (int)buttonId); }
            public bool GetPressDown(EVRButtonId buttonId) { return GetPressDown(1ul << (int)buttonId); }
            public bool GetPressUp(EVRButtonId buttonId) { return GetPressUp(1ul << (int)buttonId); }

            public bool GetTouch(ulong buttonMask) { Update(); return (currentState.ulButtonTouched & buttonMask) != 0; }
            public bool GetTouchDown(ulong buttonMask) { Update(); return (currentState.ulButtonTouched & buttonMask) != 0 && (previousState.ulButtonTouched & buttonMask) == 0; }
            public bool GetTouchUp(ulong buttonMask) { Update(); return (currentState.ulButtonTouched & buttonMask) == 0 && (previousState.ulButtonTouched & buttonMask) != 0; }

            public bool GetTouch(EVRButtonId buttonId) { return GetTouch(1ul << (int)buttonId); }
            public bool GetTouchDown(EVRButtonId buttonId) { return GetTouchDown(1ul << (int)buttonId); }
            public bool GetTouchUp(EVRButtonId buttonId) { return GetTouchUp(1ul << (int)buttonId); }

            public Vector2 GetAxis(bool usePrevState, EVRButtonId buttonId = EVRButtonId.k_EButton_SteamVR_Touchpad)
            {
                return GetAxis(buttonId, usePrevState);
            }

            public Vector2 GetAxis(EVRButtonId buttonId = EVRButtonId.k_EButton_SteamVR_Touchpad, bool usePrevState = false)
            {
                Update();
                VRControllerState_t state = usePrevState ? previousState : currentState;
                var axisId = (uint)buttonId - (uint)EVRButtonId.k_EButton_Axis0;
                switch (axisId)
                {
                    case 0: return new Vector2(state.rAxis0.x, state.rAxis0.y);
                    case 1: return new Vector2(state.rAxis1.x, state.rAxis1.y);
                    case 2: return new Vector2(state.rAxis2.x, state.rAxis2.y);
                    case 3: return new Vector2(state.rAxis3.x, state.rAxis3.y);
                    case 4: return new Vector2(state.rAxis4.x, state.rAxis4.y);
                    default: return Vector2.zero;
                }
            }

            public float hairTriggerDelta = 0.1f; // amount trigger must be pulled or released to change state
            private float hairTriggerLimit;
            private bool hairTriggerState;
            private bool hairTriggerPrevState;

            private void UpdateHairTrigger()
            {
                hairTriggerPrevState = hairTriggerState;
                var value = currentState.rAxis1.x; // trigger
                if (hairTriggerState)
                {
                    if (value < hairTriggerLimit - hairTriggerDelta || value <= 0.0f)
                        hairTriggerState = false;
                }
                else
                {
                    if (value > hairTriggerLimit + hairTriggerDelta || value >= 1.0f)
                        hairTriggerState = true;
                }
                hairTriggerLimit = hairTriggerState ? Mathf.Max(hairTriggerLimit, value) : Mathf.Min(hairTriggerLimit, value);
            }

            public float GetTriggerValue(bool usePrevState) { Update(); return usePrevState ? previousState.rAxis1.x : currentState.rAxis1.x; }
            public float GetTriggerValue() { Update(); return currentState.rAxis1.x; }
            public bool GetHairTrigger() { Update(); return hairTriggerState; }
            public bool GetHairTriggerDown() { Update(); return hairTriggerState && !hairTriggerPrevState; }
            public bool GetHairTriggerUp() { Update(); return !hairTriggerState && hairTriggerPrevState; }

            public Vector2 GetPadPressVector()
            {
                return GetPress(ControllerButton.Pad) ? (GetAxis() - padDownAxis) : Vector2.zero;
            }

            public Vector2 GetPadTouchVector()
            {
                return GetPress(ControllerButton.PadTouch) ? (GetAxis() - padTouchDownAxis) : Vector2.zero;
            }

            public VRControllerState_t GetCurrentRawState()
            {
                return currentState;
            }

            public VRControllerState_t GetPreviousRawState()
            {
                return previousState;
            }
        }
    }
}