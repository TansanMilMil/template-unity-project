using System;
using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ConfigSaveDataStoreRegistry<Tkey, TValue> : Singleton<ConfigSaveDataStoreRegistry<Tkey, TValue>>, IConfigSaveDataStoreRegistry<Tkey, TValue>, IRequireInitialize<InitModel<Tkey, TValue>>
    {
        private IStore<Tkey, TValue> configSaveDataStore;
        private Tkey storeKey;

        public void Initialize(InitModel<Tkey, TValue> initModel)
        {
            configSaveDataStore = initModel.configSaveDataStore;
            storeKey = initModel.storeKey;
        }

        public void AssertInitialized()
        {
            if (configSaveDataStore == null)
            {
                throw new InvalidOperationException("ConfigSaveDataStore is not initialized. Please call Initialize() before using this method.");
            }
            if (EqualityComparer<Tkey>.Default.Equals(storeKey))
            {
                throw new InvalidOperationException("Store key is not set. Call Initialize() first.");
            }
        }

        public IStore<Tkey, TValue> GetConfigSaveDataStore()
        {
            AssertInitialized();

            return configSaveDataStore;
        }

        public Tkey GetStoreKey()
        {
            AssertInitialized();

            return storeKey;
        }
    }

    public class InitModel<Tkey, TValue>
    {
        public IStore<Tkey, TValue> configSaveDataStore { get; private set; }
        public Tkey storeKey { get; private set; }

        public InitModel(Tkey storeKey, IStore<Tkey, TValue> store)
        {
            this.storeKey = storeKey;
            this.configSaveDataStore = store;
        }
    }
}
