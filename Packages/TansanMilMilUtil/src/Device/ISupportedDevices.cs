using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public interface ISupportedDevices
    {
        List<DeviceType> GetSupportedDevices();
        bool IsSupported(DeviceType deviceType);
    }
}