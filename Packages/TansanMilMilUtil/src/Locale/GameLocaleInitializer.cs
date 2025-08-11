using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TansanMilMil.Util;
using R3;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TansanMilMil.Util
{
    public class GameLocaleInitializer<StoreKey, StoreValue> : SingletonMonoBehaviour<GameLocaleInitializer<StoreKey, StoreValue>>
    {
        private IPlayerConfigManager playerConfigManager => PlayerConfigManager.GetInstance();
        private ILocaleManager localeManager => LocaleManager.GetInstance();
        private IConfigSaveDataManager<StoreKey, StoreValue> configSaveDataManager => ConfigSaveDataManager<StoreKey, StoreValue>.GetInstance();

        private void Start()
        {
            InitGameLocaleAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask InitGameLocaleAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            await WaitDependenciesInitAsync(cToken);

            cToken.ThrowIfCancellationRequested();
            string localeInConfig = playerConfigManager.GetConfig().cultureInfoName;
            localeManager.SetLocale(localeInConfig);

            Destroy(gameObject);
        }

        private async UniTask WaitDependenciesInitAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            Debug.Log("Waiting for LocaleManager and ConfigSaveDataManager initialization...");
            await UniTask.WaitUntil(() =>
                LocalizationSettings.InitializationOperation.IsDone &&
                localeManager.IsInitialized, cancellationToken: cToken);

            cToken.ThrowIfCancellationRequested();
            Debug.Log("Waiting for ConfigSaveDataManager to load initial data...");
            await UniTask.WaitUntil(() => configSaveDataManager.LoadedInit, cancellationToken: cToken);
        }
    }
}
