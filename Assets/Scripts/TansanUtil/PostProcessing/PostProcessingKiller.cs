using UnityEngine;

namespace TansanMilMil.Util
{
    public class PostProcessingKiller : MonoBehaviour
    {
        [SerializeField] private GameObject globalVolume;

        void Start()
        {
            Kill();
        }

        private void Kill()
        {
            switch (UnityEngine.Device.Application.platform)
            {
                // PostProcessingを無効化して処理を軽くする
                case RuntimePlatform.Android:
                    globalVolume.SetActive(false);
                    Debug.Log("PostProcessingKiller: PostProcessing has been killed.");
                    break;
                default:
                    break;
            }
        }
    }
}