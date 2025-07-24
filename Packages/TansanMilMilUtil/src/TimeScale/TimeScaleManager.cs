using UnityEngine;

namespace TansanMilMil.Util
{
    public static class TimeScaleManager
    {
        private static float addTimeVal = 0f;
        private const float DefaultTimeScale = 1.0f;
        private const float MaxTimeScale = 10.0f;
        private const float MinTimeScale = 0;

        public static void AddTimeScale(float addValue)
        {
            addTimeVal = addTimeVal + addValue;
            Time.timeScale = Mathf.Clamp(DefaultTimeScale + addTimeVal, MinTimeScale, MaxTimeScale);
        }

        public static void SetTimeScale(float timeScale)
        {
            addTimeVal = timeScale - DefaultTimeScale;
            Time.timeScale = Mathf.Clamp(timeScale, MinTimeScale, MaxTimeScale);
        }

        public static void RetrievePrevAddTimeVal()
        {
            Time.timeScale = Mathf.Clamp(DefaultTimeScale + addTimeVal, MinTimeScale, MaxTimeScale);
        }

        public static void ResetTimeScale()
        {
            addTimeVal = 0;
            Time.timeScale = Mathf.Clamp(DefaultTimeScale, MinTimeScale, MaxTimeScale);
        }

        public static void ResetTimeScaleTemporarily()
        {
            Time.timeScale = Mathf.Clamp(DefaultTimeScale, MinTimeScale, MaxTimeScale);
        }
    }
}
