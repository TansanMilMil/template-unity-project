using UnityEngine;

namespace TansanMilMil.Util
{
    public class ScreenResolutionTuner : MonoBehaviour
    {
        private PlatformScreenResolutionConfig resolutionConfig;
        private static bool SetCompleted = false;

        void Start()
        {
            resolutionConfig = ScreenResolutionManager.GetConfig();

            SetResolution();
        }

        private void SetResolution()
        {
            if (SetCompleted)
                return;

            ScreenResolutionManager.Initialize(resolutionConfig);
            ScreenResolutionManager.ApplyResolutionForCurrentPlatform();

            SetCompleted = true;
        }
    }
}
