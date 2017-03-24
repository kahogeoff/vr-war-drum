//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Pointer3D;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Vive
{
    public static class VivePointerEventDataExtension
    {
        public static bool IsViveButton(this PointerEventData eventData, HandRole hand)
        {
            if (eventData == null) { return false; }

            if (!(eventData is VivePointerEventData)) { return false; }

            var viveEvent = eventData as VivePointerEventData;
            return viveEvent.hand == hand;
        }

        public static bool IsViveButton(this PointerEventData eventData, ControllerButton button)
        {
            if (eventData == null) { return false; }

            if (!(eventData is VivePointerEventData)) { return false; }

            var viveEvent = eventData as VivePointerEventData;
            return viveEvent.viveButton == button;
        }

        public static bool IsViveButton(this PointerEventData eventData, HandRole hand, ControllerButton button)
        {
            if (eventData == null) { return false; }

            if (!(eventData is VivePointerEventData)) { return false; }

            var viveEvent = eventData as VivePointerEventData;
            return viveEvent.hand == hand && viveEvent.viveButton == button;
        }

        public static bool TryGetViveButtonEventData(this PointerEventData eventData, out VivePointerEventData viveEventData)
        {
            viveEventData = null;

            if (eventData == null) { return false; }

            if (!(eventData is VivePointerEventData)) { return false; }

            viveEventData = eventData as VivePointerEventData;
            return true;
        }
    }

    // Custom PointerEventData implement for Vive controller.
    public class VivePointerEventData : Pointer3DEventData
    {
        public readonly HandRole hand;
        public readonly ControllerButton viveButton;

        public VivePointerEventData(Pointer3DRaycaster ownerRaycaster, EventSystem eventSystem, HandRole hand, ControllerButton viveButton, InputButton mouseButton) : base(ownerRaycaster, eventSystem)
        {
            this.hand = hand;
            this.viveButton = viveButton;
            button = mouseButton;
        }

        public override bool GetPress() { return ViveInput.GetPress(hand, viveButton); }

        public override bool GetPressDown() { return ViveInput.GetPressDown(hand, viveButton); }

        public override bool GetPressUp() { return ViveInput.GetPressUp(hand, viveButton); }
    }
}