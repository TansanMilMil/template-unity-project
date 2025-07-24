using System;
using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlatformScreenResolutionConfig
    {
        private List<PlatformScreenResolutionItem> platformResolutions = new List<PlatformScreenResolutionItem>();

        public PlatformScreenResolutionConfig(List<PlatformScreenResolutionItem> resolutions)
        {
            platformResolutions = resolutions ?? throw new ArgumentNullException(nameof(resolutions), "Platform resolutions cannot be null");
        }

        public PlatformScreenResolutionItem GetResolutionForPlatform(RuntimePlatform platform)
        {
            var item = platformResolutions.Find(x => x.platform == platform);
            if (item == null)
            {
                throw new Exception($"No screen resolution configuration found for platform: {platform}");
            }
            return item;
        }

        public bool HasResolutionForPlatform(RuntimePlatform platform)
        {
            return platformResolutions.Exists(x => x.platform == platform);
        }
    }

    [Serializable]
    public class PlatformScreenResolutionItem
    {
        public RuntimePlatform platform;
        [Range(0.1f, 1.0f)]
        public float widthRatio = 1f;
        [Range(0.1f, 1.0f)]
        public float heightRatio = 1f;

        public PlatformScreenResolutionItem(RuntimePlatform platform, float widthRatio, float heightRatio)
        {
            this.platform = platform;
            this.widthRatio = widthRatio;
            this.heightRatio = heightRatio;
        }
    }
}
