using UnityEngine;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
namespace TansanMilMil.Util
{
    public class DebugSpeedUp : MonoBehaviour
    {
        private float timeScale = 1.0f;
        private bool toggle = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                toggle = !toggle;
                if (toggle)
                {
                    timeScale = 3.0f;
                }
                else
                {
                    timeScale = 1.0f;
                }
                TimeScaleManager.SetTimeScale(timeScale);
            }
        }
    }
}
#endif
