using Codice.CM.WorkspaceServer.DataStore.WkTree;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ConfigSaveDataManager : SingletonMonoBehaviour<ConfigSaveDataManager>
    {
        private Subject<bool> _loadCompleted = new Subject<bool>();
        private Subject<bool> _saveCompleted = new Subject<bool>();
        public Observable<bool> LoadCompleted => _loadCompleted;
        public Observable<bool> SaveCompleted => _saveCompleted;
        private bool loadedInit = false;

        public void Save()
        {
            Debug.Log("ConfigSaveDataManager: now saving...");

            ConfigSaveData model = new ConfigSaveDataBuilder()
                .playerConfig(PlayerConfigManager.GetInstance().GetConfig())
                .Build();
            string json = JsonUtility.ToJson(model);
            PlayerPrefs.SetString(PlayerPrefsKeys.ConfigSaveData, json);
            Debug.Log("ConfigSaveDataManager: save completed!");

            AfterSave();
        }

        private void AfterSave()
        {
            _saveCompleted.OnNext(true);
        }

        protected void Load()
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKeys.ConfigSaveData);
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

        public void LoadIfRequired()
        {
            if (!loadedInit)
            {
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

            PlayerConfigManager.GetInstance().SetConfig(saveData.playerConfig);
        }
    }
}
