using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ConfigSaveDataManager : SaveDataManagerWithIsLoaded
    {
        public static ConfigSaveDataManager Instance = new ConfigSaveDataManager();

        private ConfigSaveDataManager() { }

        public static ConfigSaveDataManager GetInstance()
        {
            return Instance;
        }

        public override void SaveInner()
        {
            Debug.Log("ConfigSaveDataManager: now saving...");

            ConfigSaveData model = new ConfigSaveDataBuilder()
                .playerConfig(PlayerConfigManager.GetInstance().GetConfig())
                .Build();
            string json = JsonUtility.ToJson(model);
            PlayerPrefs.SetString(PlayerPrefsKeys.ConfigSaveData, json);
            Debug.Log("ConfigSaveDataManager: save completed!");
        }

        protected override void LoadInner()
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKeys.ConfigSaveData);
            ConfigSaveData saveData = JsonUtility.FromJson<ConfigSaveData>(json);
            SetStaticParams(saveData);
            Debug.Log("ConfigSaveDataManager: load completed!");
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