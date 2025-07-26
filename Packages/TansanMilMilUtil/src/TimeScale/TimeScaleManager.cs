using UnityEngine;

namespace TansanMilMil.Util
{
    public class TimeScaleManager : Singleton<TimeScaleManager>, ITimeScaleManager
    {
        private float addTimeVal = 0f;
        private const float DefaultTimeScale = 1.0f;
        private const float MaxTimeScale = 10.0f;
        private const float MinTimeScale = 0;

        public void AddTimeScale(float addValue)
        {
            addTimeVal = addTimeVal + addValue;
            Time.timeScale = Mathf.Clamp(DefaultTimeScale + addTimeVal, MinTimeScale, MaxTimeScale);
        }

        public void SetTimeScale(float timeScale)
        {
            addTimeVal = timeScale - DefaultTimeScale;
            Time.timeScale = Mathf.Clamp(timeScale, MinTimeScale, MaxTimeScale);
        }

        public void RetrievePrevAddTimeVal()
        {
            Time.timeScale = Mathf.Clamp(DefaultTimeScale + addTimeVal, MinTimeScale, MaxTimeScale);
        }

        public void ResetTimeScale()
        {
            addTimeVal = 0;
            Time.timeScale = Mathf.Clamp(DefaultTimeScale, MinTimeScale, MaxTimeScale);
        }

        public void ResetTimeScaleTemporarily()
        {
            Time.timeScale = Mathf.Clamp(DefaultTimeScale, MinTimeScale, MaxTimeScale);
        }
    }
}
