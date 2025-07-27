using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireInitializeSingleton]
    public class FrameRateManager : Singleton<FrameRateManager>, IFrameRateManager
    {
        private PlatformFrameRateConfig config;

        public void Initialize(PlatformFrameRateConfig frameRateConfig)
        {
            config = frameRateConfig;
        }

        public void ApplyFrameRateForCurrentPlatform()
        {
            if (config == null)
            {
                Debug.LogError("FrameRateManager is not initialized. Please call Initialize() before applying frame rate.");
                return;
            }

            var currentPlatform = UnityEngine.Device.Application.platform;
            ApplyFrameRateForPlatform(currentPlatform);
        }

        private void ApplyFrameRateForPlatform(RuntimePlatform platform)
        {
            if (config == null)
            {
                Debug.LogError("FrameRateManager is not initialized.");
                return;
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
