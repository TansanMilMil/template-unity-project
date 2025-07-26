using Codice.CM.Common.Matcher;
using Codice.CM.WorkspaceServer.DataStore.WkTree;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class ConfigSaveDataManager : SingletonMonoBehaviour<ConfigSaveDataManager>, IConfigSaveDataManager
    {
        private Subject<bool> _loadCompleted = new Subject<bool>();
        private Subject<bool> _saveCompleted = new Subject<bool>();
        public Observable<bool> LoadCompleted => _loadCompleted;
        public Observable<bool> SaveCompleted => _saveCompleted;
        private bool loadedInit = false;
        public bool LoadedInit => loadedInit;
        private IKVStore store;
        private string storeKey;
        private IPlayerConfigManager playerConfigManager => PlayerConfigManager.GetInstance();
        private IConfigSaveDataStoreRegistry configSaveDataStoreRegistry => ConfigSaveDataStoreRegistry.GetInstance();

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
            store.Upsert(storeKey, json);
            Debug.Log("ConfigSaveDataManager: save completed!");

            AfterSave();
        }

        private void AfterSave()
        {
            _saveCompleted.OnNext(true);
        }

        protected void Load()
        {
            string json = store.Select(storeKey);
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
