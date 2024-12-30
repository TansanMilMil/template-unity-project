using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public static class SupportedDevices
    {
        /// <summary>
        /// 対応中のデバイスを保持する。対応したらListから外していく予定。
        /// </summary>
        private static readonly List<DeviceType> Devices = new List<DeviceType>
        {
            DeviceType.Desktop,
            DeviceType.Handheld,
        };

        public static bool IsSupported(DeviceType deviceType)
        {
            return Devices.Contains(deviceType);
        }
    }
}