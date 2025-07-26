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
    /// <summary>
    /// ゲーム全体のロケール設定を初期化するコンポーネント
    /// </summary>
    [DefaultExecutionOrder(-10)]
    public class GameLocaleInitializer : SingletonMonoBehaviour<GameLocaleInitializer>
    {
        private IPlayerConfigManager playerConfigManager => PlayerConfigManager.GetInstance();
        private ILocaleManager localeManager => LocaleManager.GetInstance();
        private IConfigSaveDataManager configSaveDataManager => ConfigSaveDataManager.GetInstance();

        private void Awake()
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

            gameObject.SetActive(false);
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
