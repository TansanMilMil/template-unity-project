using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class FrameRateManager : Singleton<FrameRateManager>, IFrameRateManager, IRequireInitialize<PlatformFrameRateConfig>
    {
        private PlatformFrameRateConfig config;

        public void Initialize(PlatformFrameRateConfig frameRateConfig)
        {
            config = frameRateConfig;
        }

        public void AssertInitialized()
        {
            if (config == null)
            {
                throw new InvalidOperationException("FrameRateManager has not been initialized. Please call Initialize() before using this method.");
            }
        }

        public void ApplyFrameRateForCurrentPlatform()
        {
            AssertInitialized();

            var currentPlatform = UnityEngine.Device.Application.platform;
            ApplyFrameRateForPlatform(currentPlatform);
        }

        private void ApplyFrameRateForPlatform(RuntimePlatform platform)
        {
            AssertInitialized();

            if (!config.HasPlatformConfig(platform))
            {
                Debug.LogWarning($"No frame rate configuration found for platform {platform}. Thus, nothing to do.");
                return;
            }

            int targetFrameRate = config.GetFrameRateForPlatform(platform);
            if (targetFrameRate > 0)
            {
                Application.targetFrameRate = targetFrameRate;
                Debug.Log($"Set Frame Rate to {targetFrameRate} FPS for {platform}");
            }
            else
            {
                Debug.Log($"No frame rate configuration found for {platform}, using default settings");
            }
        }
    }
}
