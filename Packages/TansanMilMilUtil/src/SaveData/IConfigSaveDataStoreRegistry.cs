namespace TansanMilMil.Util
{
    public interface IConfigSaveDataStoreRegistry
    {
        void Initialize(IKVStore store, string key);
        IKVStore GetConfigSaveDataStore();
        string GetStoreKey();
    }
}