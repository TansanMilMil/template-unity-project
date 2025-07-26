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
        [SerializeField]
        private float speedUpFactor = 3.0f;
        private const float DefaultSpeed = 1;
        private ITimeScaleManager timeScaleManager => TimeScaleManager.GetInstance();

        void Update()
        {
            if (Input.GetKeyDown(triggerKey))
            {
                toggle = !toggle;
                if (toggle)
                {
                    timeScale = speedUpFactor;
                }
                else
                {
                    timeScale = DefaultSpeed;
                }
                timeScaleManager.SetTimeScale(timeScale);
                Debug.Log($"DebugSpeedUp: Time scale set to {timeScale}.");
            }
        }
    }
}
#endif
