//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

namespace HTC.UnityPlugin.Vive
{
    /// <summary>
    /// Defines roles for those devices that have buttons
    /// </summary>
    public enum HandRole
    {
        RightHand,
        LeftHand,
        Controller3,
        Controller4,
        Controller5,
        Controller6,
        Controller7,
        Controller8,
        Controller9,
        Controller10,
        Controller11,
        Controller12,
        Controller13,
        Controller14,
        Controller15,
    }

    public static class ConvertRoleExtension
    {
        /// <summary>
        /// Mapping from HandRole to DeviceRole
        /// </summary>
        public static DeviceRole ToDeviceRole(this HandRole role)
        {
            switch (role)
            {
                case HandRole.RightHand: return DeviceRole.RightHand;
                case HandRole.LeftHand: return DeviceRole.LeftHand;
                case HandRole.Controller3: return DeviceRole.Controller3;
                case HandRole.Controller4: return DeviceRole.Controller4;
                case HandRole.Controller5: return DeviceRole.Controller5;
                case HandRole.Controller6: return DeviceRole.Controller6;
                case HandRole.Controller7: return DeviceRole.Controller7;
                case HandRole.Controller8: return DeviceRole.Controller8;
                case HandRole.Controller9: return DeviceRole.Controller9;
                case HandRole.Controller10: return DeviceRole.Controller10;
                case HandRole.Controller11: return DeviceRole.Controller11;
                case HandRole.Controller12: return DeviceRole.Controller12;
                case HandRole.Controller13: return DeviceRole.Controller13;
                case HandRole.Controller14: return DeviceRole.Controller14;
                case HandRole.Controller15: return DeviceRole.Controller15;
                default: return (DeviceRole)((int)DeviceRole.Hmd - 1); // returns invalid value
            }
        }
    }
}