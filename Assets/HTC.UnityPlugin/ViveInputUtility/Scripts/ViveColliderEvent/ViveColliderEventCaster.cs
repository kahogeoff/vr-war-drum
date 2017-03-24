//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;

namespace HTC.UnityPlugin.Vive
{
    public class ViveColliderEventCaster : ColliderEventCaster
    {
        public enum ButtonEventSource
        {
            BothHands,
            RightHandOnly,
            LeftHandOnly,
            None,
        }

        [SerializeField]
        private ButtonEventSource buttonEventSource = ButtonEventSource.RightHandOnly;

        protected virtual void Start()
        {
            // customize your button events here
            switch (buttonEventSource)
            {
                case ButtonEventSource.BothHands:
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Trigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Pad));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Grip));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.PadTouch));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Menu));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.HairTrigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.FullTrigger));
                    axisEventDataList.Add(new ViveColliderTriggerValueEventData(this, HandRole.RightHand));
                    axisEventDataList.Add(new ViveColliderPadAxisEventData(this, HandRole.RightHand));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Trigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Pad));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Grip));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.PadTouch));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Menu));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.HairTrigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.FullTrigger));
                    axisEventDataList.Add(new ViveColliderTriggerValueEventData(this, HandRole.LeftHand));
                    axisEventDataList.Add(new ViveColliderPadAxisEventData(this, HandRole.LeftHand));
                    break;
                case ButtonEventSource.RightHandOnly:
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Trigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Pad));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Grip));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.PadTouch));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.Menu));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.HairTrigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.RightHand, ControllerButton.FullTrigger));
                    axisEventDataList.Add(new ViveColliderTriggerValueEventData(this, HandRole.RightHand));
                    axisEventDataList.Add(new ViveColliderPadAxisEventData(this, HandRole.RightHand));
                    break;
                case ButtonEventSource.LeftHandOnly:
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Trigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Pad));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Grip));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.PadTouch));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.Menu));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.HairTrigger));
                    buttonEventDataList.Add(new ViveColliderButtonEventData(this, HandRole.LeftHand, ControllerButton.FullTrigger));
                    axisEventDataList.Add(new ViveColliderTriggerValueEventData(this, HandRole.LeftHand));
                    axisEventDataList.Add(new ViveColliderPadAxisEventData(this, HandRole.LeftHand));
                    break;
            }
        }
    }
}