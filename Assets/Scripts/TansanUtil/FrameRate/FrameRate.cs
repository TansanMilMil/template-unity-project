using UnityEngine;

namespace TansanMilMil.Util
{
    public class FrameRate : MonoBehaviour
    {
        private static bool SetCompleted = false;

        void Start()
        {
            SetFrameRate();
        }

        private void SetFrameRate()
        {
            if (SetCompleted) return;

            switch (UnityEngine.Device.Application.platform)
            {
                case RuntimePlatform.Android:
                    Application.targetFrameRate = 60;
                    break;
                default:
                    break;
            }
            SetCompleted = true;
        }
    }
}