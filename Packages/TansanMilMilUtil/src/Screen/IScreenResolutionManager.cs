namespace TansanMilMil.Util
{
    public interface IScreenResolutionManager
    {
        void Initialize(PlatformScreenResolutionConfig resolutionConfig);
        void ApplyResolutionForCurrentPlatform();
        PlatformScreenResolutionConfig GetConfig();
    }
}