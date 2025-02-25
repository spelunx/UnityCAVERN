using UnityEngine;
using UnityEngine.Events;

namespace Spelunx.OVRT
{
    public abstract class OVRT_TrackedDevice : MonoBehaviour
    {
        public Transform origin;

        public int DeviceIndex { get; protected set; } = -1;
        public bool IsValid { get; protected set; }
        public bool IsConnected { get; protected set; }

        [HideInInspector]
        public UnityEvent<int> onDeviceIndexChanged;

        protected UnityAction<int, bool> _onDeviceConnectedAction;
    }
}