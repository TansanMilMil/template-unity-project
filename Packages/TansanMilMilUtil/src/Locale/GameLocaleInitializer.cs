using System;
using Cysharp.Threading.Tasks;
using TansanMilMil.Util;
using R3;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TansanMilMil.Util
{
    /// <summary>
    /// ゲーム全体のロケール設定を初期化するコンポーネント
    /// </summary>
    [DefaultExecutionOrder(-10)]
    public class GameLocaleInitializer : SingletonMonoBehaviour<GameLocaleInitializer>
    {
        private void Awake()
        {
            InitGameLocaleAsync().Forget();
        }

        private async UniTask InitGameLocaleAsync()
        {
            await WaitDependenciesInitAsync();

            string localeInConfig = PlayerConfigManager.GetInstance().GetConfig().cultureInfoName;
            LocaleManager.GetInstance().SetLocale(localeInConfig);

            gameObject.SetActive(false);
        }

        private async UniTask WaitDependenciesInitAsync()
        {
            Debug.Log("Waiting for LocaleManager and ConfigSaveDataManager initialization...");
            await UniTask.WaitUntil(() =>
                LocalizationSettings.InitializationOperation.IsDone &&
                LocaleManager.GetInstance().IsInitialized);

            Debug.Log("Waiting for ConfigSaveDataManager to load initial data...");
            await UniTask.WaitUntil(() => ConfigSaveDataManager.GetInstance().LoadedInit);
        }
    }
}
