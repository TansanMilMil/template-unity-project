using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    public static class ConfigSaveDataStoreRegistryInitializer
    {
        public static void Initialize()
        {
            var store = new PlayerPrefsStore();
            var storeKey = "projectConfig";
            var initModel = new InitModel<string, string>(storeKey, store);

            ConfigSaveDataStoreRegistry<string, string>.GetInstance().Initialize(initModel);

            Debug.Log("ConfigSaveDataStoreRegistry initialized with project-specific store");
        }
    }
}
