//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Pointer3D;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Vive
{
    [AddComponentMenu("HTC/Vive/Vive Raycaster")]
    // Custom Pointer3DRaycaster implement for Vive controller.
    public class ViveRaycaster : Pointer3DRaycaster
    {
        public enum ButtonEventSource
        {
            AllButtons,
            RightHandOnly,
            LeftHandOnly,
            None,
        }

        [SerializeField]
        private ButtonEventSource buttonEventSource = ButtonEventSource.RightHandOnly;
        public float scrollDeltaScale = 50f;

        protected override void Start()
        {
            base.Start();
            switch (buttonEventSource)
            {
                case ButtonEventSource.AllButtons:
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.RightHand, ControllerButton.Trigger, PointerEventData.InputButton.Left));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.RightHand, ControllerButton.Pad, PointerEventData.InputButton.Middle));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.RightHand, ControllerButton.Grip, PointerEventData.InputButton.Right));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.LeftHand, ControllerButton.Trigger, PointerEventData.InputButton.Left));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.LeftHand, ControllerButton.Pad, PointerEventData.InputButton.Middle));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.LeftHand, ControllerButton.Grip, PointerEventData.InputButton.Right));
                    break;
                case ButtonEventSource.RightHandOnly:
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.RightHand, ControllerButton.Trigger, PointerEventData.InputButton.Left));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.RightHand, ControllerButton.Pad, PointerEventData.InputButton.Middle));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.RightHand, ControllerButton.Grip, PointerEventData.InputButton.Right));
                    break;
                case ButtonEventSource.LeftHandOnly:
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.LeftHand, ControllerButton.Trigger, PointerEventData.InputButton.Left));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.LeftHand, ControllerButton.Pad, PointerEventData.InputButton.Middle));
                    buttonEventDataList.Add(new VivePointerEventData(this, EventSystem.current, HandRole.LeftHand, ControllerButton.Grip, PointerEventData.InputButton.Right));
                    break;
            }
        }

        public override Vector2 GetScrollDelta()
        {
            var delta = Vector2.zero;
            switch (buttonEventSource)
            {
                case ButtonEventSource.AllButtons:
                    delta = ViveInput.GetPadTouchDelta(HandRole.RightHand) + ViveInput.GetPadTouchDelta(HandRole.LeftHand);
                    break;
                case ButtonEventSource.RightHandOnly:
                    delta = ViveInput.GetPadTouchDelta(HandRole.RightHand);
                    break;
                case ButtonEventSource.LeftHandOnly:
                    delta = ViveInput.GetPadTouchDelta(HandRole.LeftHand);
                    break;
            }
            return delta * scrollDeltaScale;
        }
    }
}