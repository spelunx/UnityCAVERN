using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

namespace Spelunx.OVRT
{
    /// <summary>
    /// Maps tracked OpenVR poses to transform by device index.
    /// </summary>
    public sealed class OVRT_TrackedObject : OVRT_TrackedDevice
    {
        public enum EIndex
        {
            None = -1,
            Hmd = (int)OpenVR.k_unTrackedDeviceIndex_Hmd,
            Device1,
            Device2,
            Device3,
            Device4,
            Device5,
            Device6,
            Device7,
            Device8,
            Device9,
            Device10,
            Device11,
            Device12,
            Device13,
            Device14,
            Device15,
            Device16
        }

        public EIndex index;
        [Tooltip("If not set, relative to parent")]

        private UnityAction<TrackedDevicePose_t[]> _onNewPosesAction;

        private void OnDeviceConnected(int index, bool connected)
        {
            if ((int)this.index == index)
            {
                DeviceIndex = index;
                IsConnected = connected;

                onDeviceIndexChanged.Invoke(DeviceIndex);
            }
        }

        private void OnNewPoses(TrackedDevicePose_t[] poses)
        {
            if (index == EIndex.None)
                return;

            var i = DeviceIndex;

            IsValid = false;

            if (i < 0 || poses.Length <= i)
                return;

            if (!poses[i].bDeviceIsConnected)
                return;

            if (!poses[i].bPoseIsValid)
                return;

            IsValid = true;

            var pose = new OVRT_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            if (origin != null)
            {
                transform.position = origin.transform.TransformPoint(pose.pos);
                transform.rotation = origin.rotation * pose.rot;
            }
            else
            {
                transform.localPosition = pose.pos;
                transform.localRotation = pose.rot;
            }
        }

        private void Awake()
        {
            _onNewPosesAction += OnNewPoses;
            _onDeviceConnectedAction += OnDeviceConnected;
        }

        private void OnEnable()
        {
            DeviceIndex = (int)index;
            onDeviceIndexChanged.Invoke(DeviceIndex);

            OVRT_Events.NewPoses.AddListener(_onNewPosesAction);
            OVRT_Events.TrackedDeviceConnected.AddListener(_onDeviceConnectedAction);
        }

        private void OnDisable()
        {
            OVRT_Events.NewPoses.RemoveListener(_onNewPosesAction);
            IsValid = false;
            IsConnected = false;
        }
    }
}