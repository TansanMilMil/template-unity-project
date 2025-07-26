namespace TansanMilMil.Util
{
    public interface IFrameRateManager
    {
        void Initialize(PlatformFrameRateConfig frameRateConfig);
        void ApplyFrameRateForCurrentPlatform();
    }
}