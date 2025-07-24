using System;
using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlatformFrameRateConfig : MonoBehaviour
    {
        [Header("Platform Frame Rate Configuration")]
        [SerializeField]
        private List<PlatformFrameRateItem> platformFrameRates = new List<PlatformFrameRateItem>();

        public int GetFrameRateForPlatform(RuntimePlatform platform)
        {
            var item = platformFrameRates.Find(x => x.platform == platform);
            if (item == null)
            {
                throw new KeyNotFoundException($"Frame rate configuration for platform {platform} not found.");
            }
            return item.frameRate;
        }

        public bool HasPlatformConfig(RuntimePlatform platform)
        {
            return platformFrameRates.Exists(x => x.platform == platform);
        }
    }

    [Serializable]
    public class PlatformFrameRateItem
    {
        public RuntimePlatform platform;
        public int frameRate;

        public PlatformFrameRateItem(RuntimePlatform platform, int frameRate)
        {
            this.platform = platform;
            this.frameRate = frameRate;
        }
    }
}
