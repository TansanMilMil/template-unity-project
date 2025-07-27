namespace TansanMilMil.Util
{
    public interface ISupportedDevicesProviderRegistry
    {
        void Initialize(ISupportedDevicesProvider provider);
        ISupportedDevicesProvider GetProvider();
        bool IsProviderRegistered();
    }
}
