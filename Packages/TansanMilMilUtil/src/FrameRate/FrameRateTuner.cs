using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireComponent(typeof(PlatformFrameRateConfig))]
    public class FrameRateTuner : MonoBehaviour
    {
        [SerializeField]
        private PlatformFrameRateConfig frameRateConfig;
        private static bool SetCompleted = false;

        void Start()
        {
            SetFrameRate();
            Destroy(this);
        }

        private void SetFrameRate()
        {
            if (SetCompleted)
                return;

            if (frameRateConfig != null)
            {
                FrameRateManager.Initialize(frameRateConfig);
                FrameRateManager.ApplyFrameRateForCurrentPlatform();
            }

            SetCompleted = true;
        }
    }
}
