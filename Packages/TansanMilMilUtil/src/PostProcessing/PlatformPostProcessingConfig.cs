using System;
using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlatformPostProcessingConfig : Singleton<PlatformPostProcessingConfig>, IPlatformPostProcessingConfig, IRequireInitialize<List<PlatformPostProcessingItem>>
    {
        private List<PlatformPostProcessingItem> platformPostProcessingSettings = new List<PlatformPostProcessingItem>();

        public void Initialize(List<PlatformPostProcessingItem> settings)
        {
            platformPostProcessingSettings = settings;
        }

        public void AssertInitialized()
        {
            if (platformPostProcessingSettings == null || platformPostProcessingSettings.Count == 0)
            {
                throw new InvalidOperationException("PlatformPostProcessingConfig is not initialized. Please call Initialize() before using this method.");
            }
        }

        public bool ShouldDisablePostProcessingForPlatform(RuntimePlatform platform)
        {
            AssertInitialized();

            var item = platformPostProcessingSettings.Find(x => x.platform == platform);
            return item != null && item.disablePostProcessing;
        }

        public bool HasPlatformConfig(RuntimePlatform platform)
        {
            AssertInitialized();

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
