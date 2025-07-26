using UnityEngine;

namespace TansanMilMil.Util
{
    public class FrameRateManager : Singleton<FrameRateManager>, IFrameRateManager
    {
        private PlatformFrameRateConfig config;
        private bool isInitialized = false;

        public void Initialize(PlatformFrameRateConfig frameRateConfig)
        {
            config = frameRateConfig;
            isInitialized = true;
        }

        public void ApplyFrameRateForCurrentPlatform()
        {
            if (!isInitialized)
            {
                throw new System.InvalidOperationException("FrameRateManager is not initialized.");
            }

            var currentPlatform = UnityEngine.Device.Application.platform;
            ApplyFrameRateForPlatform(currentPlatform);
        }

        private void ApplyFrameRateForPlatform(RuntimePlatform platform)
        {
            if (!isInitialized)
            {
                throw new System.InvalidOperationException("FrameRateManager is not initialized.");
            }

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
