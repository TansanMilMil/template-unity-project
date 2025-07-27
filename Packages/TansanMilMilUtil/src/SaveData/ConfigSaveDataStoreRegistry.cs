using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireInitializeSingleton]
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
                Debug.LogError("ConfigSaveDataStore is not initialized. Please call Initialize() before using GetConfigSaveDataStore().");
                return null;
            }
            return configSaveDataStore;
        }

        public Tkey GetStoreKey()
        {
            if (EqualityComparer<Tkey>.Default.Equals(storeKey))
            {
                Debug.LogError("Store key is not set. Call Initialize() first.");
                return default;
            }
            return storeKey;
        }
    }
}
