using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public interface IPlatformPostProcessingConfig
    {
        void Initialize(List<PlatformPostProcessingItem> settings);
        bool ShouldDisablePostProcessingForPlatform(RuntimePlatform platform);
        bool HasPlatformConfig(RuntimePlatform platform);
    }
}