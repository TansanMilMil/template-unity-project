using UnityEngine;

namespace TansanMilMil.Util
{
    public class PostProcessingKiller : MonoBehaviour
    {
        [SerializeField] private GameObject globalVolume;
        private static bool KillCompleted = false;
        private IPlatformPostProcessingConfig postProcessingConfig => PlatformPostProcessingConfig.GetInstance();

        void Start()
        {
            Kill();
            Destroy(this);
        }

        private void Kill()
        {
            if (KillCompleted)
                return;

            if (postProcessingConfig != null && globalVolume != null)
            {
                RuntimePlatform currentPlatform = UnityEngine.Device.Application.platform;

                if (postProcessingConfig.ShouldDisablePostProcessingForPlatform(currentPlatform))
                {
                    globalVolume.SetActive(false);
                    Debug.Log($"PostProcessingKiller: PostProcessing has been killed for platform {currentPlatform}.");
                }
            }

            KillCompleted = true;
        }
    }
}
