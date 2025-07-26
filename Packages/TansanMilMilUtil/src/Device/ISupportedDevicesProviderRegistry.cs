namespace TansanMilMil.Util
{
    public interface ISupportedDevicesProviderRegistry
    {
        void RegisterProvider(ISupportedDevicesProvider provider);
        ISupportedDevicesProvider GetProvider();
        bool IsProviderRegistered();
    }
}