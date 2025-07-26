using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireComponent(typeof(PlatformFrameRateConfig))]
    public class FrameRateTuner : MonoBehaviour
    {
        [SerializeField]
        private PlatformFrameRateConfig frameRateConfig;
        private static bool SetCompleted = false;
        private IFrameRateManager frameRateManager => FrameRateManager.GetInstance();

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
                frameRateManager.Initialize(frameRateConfig);
                frameRateManager.ApplyFrameRateForCurrentPlatform();
            }

            SetCompleted = true;
        }
    }
}
