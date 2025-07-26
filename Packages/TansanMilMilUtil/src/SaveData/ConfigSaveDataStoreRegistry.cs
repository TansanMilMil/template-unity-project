using System.Collections.Generic;

namespace TansanMilMil.Util
{
    public class ConfigSaveDataStoreRegistry<Tkey, TValue> : Singleton<ConfigSaveDataStoreRegistry<Tkey, TValue>>, IConfigSaveDataStoreRegistry<Tkey, TValue>
    {
        private IStore<Tkey, TValue> configSaveDataStore;
        private Tkey storeKey;

        public void Initialize(IStore<Tkey, TValue> store, Tkey key)
        {
            configSaveDataStore = store;
            storeKey = key;
        }

        public IStore<Tkey, TValue> GetConfigSaveDataStore()
        {
            if (configSaveDataStore == null)
            {
                throw new System.InvalidOperationException("ConfigSaveDataStore is not initialized. Call Initialize() first.");
            }
            return configSaveDataStore;
        }

        public Tkey GetStoreKey()
        {
            if (EqualityComparer<Tkey>.Default.Equals(storeKey))
            {
                throw new System.InvalidOperationException("Store key is not set. Call Initialize() first.");
            }
            return storeKey;
        }
    }
}
