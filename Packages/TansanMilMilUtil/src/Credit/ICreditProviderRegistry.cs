namespace TansanMilMil.Util
{
    public interface ICreditProviderRegistry
    {
        void RegisterProvider(ICreditProvider provider);
        ICreditProvider GetProvider();
        bool IsProviderRegistered();
    }
}