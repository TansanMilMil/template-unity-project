using System;
using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireInitializeSingleton]
    public class PlatformPostProcessingConfig : Singleton<PlatformPostProcessingConfig>, IPlatformPostProcessingConfig
    {
        private List<PlatformPostProcessingItem> platformPostProcessingSettings = new List<PlatformPostProcessingItem>();

        public void Initialize(List<PlatformPostProcessingItem> settings)
        {
            platformPostProcessingSettings = settings;
        }

        public bool ShouldDisablePostProcessingForPlatform(RuntimePlatform platform)
        {
            if (platformPostProcessingSettings == null)
            {
                Debug.LogError("PlatformPostProcessingConfig is not initialized. Please call Initialize() before using this method.");
                return false;
            }

            var item = platformPostProcessingSettings.Find(x => x.platform == platform);
            return item != null && item.disablePostProcessing;
        }

        public bool HasPlatformConfig(RuntimePlatform platform)
        {
            if (platformPostProcessingSettings == null)
            {
                Debug.LogError("PlatformPostProcessingConfig is not initialized. Please call Initialize() before using this method.");
                return false;
            }

            return platformPostProcessingSettings.Exists(x => x.platform == platform);
        }
    }

    [Serializable]
    public class PlatformPostProcessingItem
    {
        public RuntimePlatform platform;
        public bool disablePostProcessing;

        public PlatformPostProcessingItem(RuntimePlatform platform, bool disablePostProcessing)
        {
            this.platform = platform;
            this.disablePostProcessing = disablePostProcessing;
        }
    }
}
