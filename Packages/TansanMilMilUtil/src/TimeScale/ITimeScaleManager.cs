namespace TansanMilMil.Util
{
    public interface ITimeScaleManager
    {
        void AddTimeScale(float addValue);
        void SetTimeScale(float timeScale);
        void RetrievePrevAddTimeVal();
        void ResetTimeScale();
        void ResetTimeScaleTemporarily();
    }
}