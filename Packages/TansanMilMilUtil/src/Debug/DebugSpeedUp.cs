using UnityEngine;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
namespace TansanMilMil.Util
{
    public class DebugSpeedUp : MonoBehaviour
    {
        private float timeScale = 1.0f;
        private bool toggle = false;
        [SerializeField]
        private KeyCode triggerKey = KeyCode.F2;
        private ITimeScaleManager timeScaleManager => TimeScaleManager.GetInstance();

        void Update()
        {
            if (Input.GetKeyDown(triggerKey))
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
                timeScaleManager.SetTimeScale(timeScale);
            }
        }
    }
}
#endif
