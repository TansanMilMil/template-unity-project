namespace TansanMilMil.Util
{
    public class ConfigSaveDataStoreRegistry : Singleton<ConfigSaveDataStoreRegistry>, IConfigSaveDataStoreRegistry
    {
        private IKVStore configSaveDataStore;
        private string storeKey;

        public void Initialize(IKVStore store, string key)
        {
            configSaveDataStore = store;
            storeKey = key;
        }

        public IKVStore GetConfigSaveDataStore()
        {
            if (configSaveDataStore == null)
            {
                throw new System.InvalidOperationException("ConfigSaveDataStore is not initialized. Call Initialize() first.");
            }
            return configSaveDataStore;
        }

        public string GetStoreKey()
        {
            if (string.IsNullOrEmpty(storeKey))
            {
                throw new System.InvalidOperationException("Store key is not set. Call Initialize() first.");
            }
            return storeKey;
        }
    }
}
