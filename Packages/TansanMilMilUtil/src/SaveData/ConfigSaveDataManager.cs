using Codice.CM.WorkspaceServer.DataStore.WkTree;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ConfigSaveDataManager : SingletonMonoBehaviour<ConfigSaveDataManager>
    {
        public Subject<bool> loadCompleted = new Subject<bool>();
        public Subject<bool> saveCompleted = new Subject<bool>();
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
            saveCompleted.OnNext(true);
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
            loadCompleted.OnNext(true);
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
