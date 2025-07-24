namespace TansanMilMil.Util
{
    public static class ConfigSaveDataStoreRegistry
    {
        private static IKVStore configSaveDataStore;
        private static string storeKey;

        public static void Initialize(IKVStore store, string key)
        {
            configSaveDataStore = store;
            storeKey = key;
        }

        public static IKVStore GetConfigSaveDataStore()
        {
            if (configSaveDataStore == null)
            {
                throw new System.InvalidOperationException("ConfigSaveDataStore is not initialized. Call Initialize() first.");
            }
            return configSaveDataStore;
        }

        public static string GetStoreKey()
        {
            if (string.IsNullOrEmpty(storeKey))
            {
                throw new System.InvalidOperationException("Store key is not set. Call Initialize() first.");
            }
            return storeKey;
        }
    }
}
