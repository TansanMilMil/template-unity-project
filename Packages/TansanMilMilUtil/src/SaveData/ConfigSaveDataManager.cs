using Codice.CM.Common.Matcher;
using Codice.CM.WorkspaceServer.DataStore.WkTree;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ConfigSaveDataManager<StoreKey, StoreValue> : SingletonMonoBehaviour<ConfigSaveDataManager<StoreKey, StoreValue>>, IConfigSaveDataManager<StoreKey, StoreValue>
    {
        private Subject<bool> _loadCompleted = new Subject<bool>();
        private Subject<bool> _saveCompleted = new Subject<bool>();
        public Observable<bool> LoadCompleted => _loadCompleted;
        public Observable<bool> SaveCompleted => _saveCompleted;
        private bool loadedInit = false;
        public bool LoadedInit => loadedInit;
        private IStore<StoreKey, StoreValue> store;
        private IStoreValueConverter<StoreValue> storeValueConverter => KVStoreValueConverter<StoreValue>.GetInstance();
        private StoreKey storeKey;
        private IPlayerConfigManager playerConfigManager => PlayerConfigManager.GetInstance();
        private IConfigSaveDataStoreRegistry<StoreKey, StoreValue> configSaveDataStoreRegistry => ConfigSaveDataStoreRegistry<StoreKey, StoreValue>.GetInstance();

        protected override void OnSingletonStart()
        {
            store = configSaveDataStoreRegistry.GetConfigSaveDataStore();
            storeKey = configSaveDataStoreRegistry.GetStoreKey();

            LoadAsInit();
        }

        public void Save()
        {
            Debug.Log("ConfigSaveDataManager: now saving...");

            ConfigSaveData model = new ConfigSaveDataBuilder()
                .playerConfig(playerConfigManager.GetConfig())
                .Build();
            string json = JsonUtility.ToJson(model);

            store.CreateStoreConnection();
            store.Upsert(storeKey, storeValueConverter.Convert(json));
            store.Commit();
            store.CloseStoreConnection();
            Debug.Log("ConfigSaveDataManager: save completed!");

            AfterSave();
        }

        private void AfterSave()
        {
            _saveCompleted.OnNext(true);
        }

        protected void Load()
        {
            store.CreateStoreConnection();
            StoreValue value = store.Select(storeKey);
            store.Commit();
            store.CloseStoreConnection();

            string json = storeValueConverter.Convert(value);
            ConfigSaveData saveData = JsonUtility.FromJson<ConfigSaveData>(json);
            SetStaticParams(saveData);
            loadedInit = true;
            Debug.Log("ConfigSaveDataManager: load completed!");

            AfterLoad();
        }

        private void AfterLoad()
        {
            _loadCompleted.OnNext(true);
        }

        public void LoadAsInit()
        {
            if (!loadedInit)
            {
                Debug.Log("ConfigSaveDataManager: loading as init...");
                Load();
            }
        }

        private void SetStaticParams(ConfigSaveData saveData)
        {
            if (saveData == null)
            {
                Debug.Log("ConfigSaveDataManager: save data is null");
                return;
            }

            playerConfigManager.SetConfig(saveData.playerConfig);
        }
    }
}
