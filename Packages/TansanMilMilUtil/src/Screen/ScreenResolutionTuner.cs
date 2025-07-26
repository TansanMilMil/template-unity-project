using UnityEngine;

namespace TansanMilMil.Util
{
    public class ScreenResolutionTuner : MonoBehaviour
    {
        private PlatformScreenResolutionConfig resolutionConfig;
        private static bool SetCompleted = false;
        private IScreenResolutionManager screenResolutionManager => ScreenResolutionManager.GetInstance();

        void Start()
        {
            resolutionConfig = screenResolutionManager.GetConfig();

            SetResolution();
        }

        private void SetResolution()
        {
            if (SetCompleted)
                return;

            screenResolutionManager.Initialize(resolutionConfig);
            screenResolutionManager.ApplyResolutionForCurrentPlatform();

            SetCompleted = true;
        }
    }
}
