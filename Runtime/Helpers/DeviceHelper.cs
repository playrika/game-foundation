using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Playrika.GameFoundation.Helpers
{
    public static class DeviceHelper
    {
        private static float _tabletAspectRatio = 1.44f;

        public static DeviceType deviceType
        {
            get
            {
#if UNITY_IOS && !UNITY_EDITOR
                if (Device.generation.ToString().Contains("iPad"))
                    return DeviceType.Tablet;

                return DeviceType.Phone;
#else
                var aspectRatio = 1f / ((float)Screen.width / Screen.height);
                return aspectRatio < _tabletAspectRatio
                    ? DeviceType.Tablet
                    : DeviceType.Phone;
#endif
            }
        }
    }
}