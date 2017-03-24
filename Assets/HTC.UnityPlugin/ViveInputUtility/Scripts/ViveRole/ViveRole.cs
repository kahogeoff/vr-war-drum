//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Utility;
using System;
using Valve.VR;

namespace HTC.UnityPlugin.Vive
{
    /// <summary>
    /// Provide static APIs to retrieve device index by semantic role
    /// Same mapping logic as SteamVR_ControllerManager does
    /// </summary>
    public static class ViveRole
    {
        public const int DEVICE_ROLE_COUNT = 16;
        public const int HAND_ROLE_COUNT = 15;

        public delegate void DeviceIndexChangedHandler(int index);

        private static bool[] controllerConnected = new bool[OpenVR.k_unMaxTrackedDeviceCount]; // controllers only
        private static uint[] roleIndice = new uint[OpenVR.k_unMaxTrackedDeviceCount - 1]; // start with RightHand
        private static DeviceIndexChangedHandler[] indexChangedHandlers = new DeviceIndexChangedHandler[OpenVR.k_unMaxTrackedDeviceCount - 1];

        static ViveRole()
        {
            for (int i = roleIndice.Length - 1; i >= 0; --i)
            {
                roleIndice[i] = OpenVR.k_unTrackedDeviceIndexInvalid;
            }

            RefreshControllerIndex(default(VREvent_t));

            for (int i = SteamVR.connected.Length - 1; i >= 0; --i)
            {
                if (SteamVR.connected[i])
                {
                    OnDeviceConnected(i, true);
                }
            }

            SteamVR_Events.DeviceConnectedAction(OnDeviceConnected).Enable(true);
            SteamVR_Events.SystemAction(EVREventType.VREvent_TrackedDeviceRoleChanged, RefreshControllerIndex).Enable(true);
        }

        private static void OnDeviceConnected(int index, bool connected)
        {
            var changed = controllerConnected[index];
            controllerConnected[index] = false;

            if (connected)
            {
                var system = OpenVR.System;
                if (system != null)
                {
                    var deviceClass = system.GetTrackedDeviceClass((uint)index);
                    if (deviceClass == ETrackedDeviceClass.Controller || deviceClass == ETrackedDeviceClass.GenericTracker)
                    {
                        controllerConnected[index] = true;
                        changed = !changed; // if we clear and set the same index, nothing has changed
                    }
                }
            }

            if (changed)
            {
                RefreshControllerIndex(default(VREvent_t));
            }
        }

        private static void RefreshControllerIndex(VREvent_t arg)
        {
            var system = OpenVR.System;
            var rightIndex = OpenVR.k_unTrackedDeviceIndexInvalid;
            var leftIndex = OpenVR.k_unTrackedDeviceIndexInvalid;
            var currentIndex = default(int);

            if (system != null)
            {
                leftIndex = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
                rightIndex = system.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
            }

            // If neither role has been assigned yet, try hooking up at least the right controller.
            if (!IsValidIndex(leftIndex) && !IsValidIndex(rightIndex))
            {
                for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; ++i)
                {
                    if (controllerConnected[i])
                    {
                        roleIndice[currentIndex++] = i;
                        break;
                    }
                }
            }
            else
            {
                roleIndice[currentIndex++] = (IsValidIndex(rightIndex) && controllerConnected[rightIndex] && rightIndex != leftIndex) ? rightIndex : OpenVR.k_unTrackedDeviceIndexInvalid;
                roleIndice[currentIndex++] = (IsValidIndex(leftIndex) && controllerConnected[leftIndex]) ? leftIndex : OpenVR.k_unTrackedDeviceIndexInvalid;

                // Assign out any additional controllers only after both left and right have been assigned.
                if (IsValidIndex(leftIndex) && IsValidIndex(rightIndex))
                {
                    for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++)
                    {
                        if (currentIndex >= roleIndice.Length) { break; }

                        if (!controllerConnected[i]) { continue; }

                        if (i != leftIndex && i != rightIndex)
                        {
                            roleIndice[currentIndex++] = i;
                        }
                    }
                }
            }

            // Reset the rest.
            while (currentIndex < roleIndice.Length)
            {
                roleIndice[currentIndex++] = OpenVR.k_unTrackedDeviceIndexInvalid;
            }
        }

        private static void SetRoleIndex(int index, uint value)
        {
            if (ChangeProp.Set(ref roleIndice[index], value) && !ReferenceEquals(indexChangedHandlers[index], null))
            {
                indexChangedHandlers[index].Invoke((int)value);
            }
        }

        public static void AddDeviceIndexChangedListener(HandRole role, DeviceIndexChangedHandler handler) { AddDeviceIndexChangedListener(role.ToDeviceRole(), handler); }

        public static void AddDeviceIndexChangedListener(DeviceRole role, DeviceIndexChangedHandler handler)
        {
            var index = (int)role;
            if (ReferenceEquals(indexChangedHandlers[index], null))
            {
                indexChangedHandlers[index] = handler;
            }
            else
            {
                indexChangedHandlers[index] += handler;
            }
        }

        public static void RemoveDeviceIndexChangedListener(HandRole role, DeviceIndexChangedHandler handler) { RemoveDeviceIndexChangedListener(role.ToDeviceRole(), handler); }

        public static void RemoveDeviceIndexChangedListener(DeviceRole role, DeviceIndexChangedHandler handler)
        {
            var index = (int)role;
            if (!ReferenceEquals(indexChangedHandlers[index], null))
            {
                indexChangedHandlers[index] -= handler;
            }
        }

        /// <summary>
        /// Returns device index of the device identified by the role
        /// Returns OpenVR.k_unTrackedDeviceIndexInvalid if the role doesn't assign to any device
        /// </summary>
        /// <returns>Current device index assigned to the role, should be tested by ViveRole.IsValidIndex before using it</returns>
        public static uint GetDeviceIndex(HandRole role) { return GetDeviceIndex(role.ToDeviceRole()); }

        /// <summary>
        /// Returns device index of the device identified by the role
        /// Returns OpenVR.k_unTrackedDeviceIndexInvalid if the role doesn't assign to any device
        /// </summary>
        /// <returns>Current device index assigned to the role, should be tested by ViveRole.IsValidIndex before using it</returns>
        public static uint GetDeviceIndex(DeviceRole role)
        {
            if (role == DeviceRole.Hmd) { return OpenVR.k_unTrackedDeviceIndex_Hmd; }

            var index = (int)role;
            if (index < 0 || index >= roleIndice.Length)
            {
                return OpenVR.k_unTrackedDeviceIndexInvalid;
            }
            else
            {
                return roleIndice[index];
            }
        }

        /// <summary>
        /// Check if the device index is valid to be used
        /// </summary>
        public static bool IsValidIndex(uint index) { return index < OpenVR.k_unMaxTrackedDeviceCount; }
    }
}