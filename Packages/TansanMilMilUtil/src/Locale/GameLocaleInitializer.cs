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
        private void Awake()
        {
            InitGameLocaleAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask InitGameLocaleAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            await WaitDependenciesInitAsync(cToken);

            cToken.ThrowIfCancellationRequested();
            string localeInConfig = PlayerConfigManager.GetInstance().GetConfig().cultureInfoName;
            LocaleManager.GetInstance().SetLocale(localeInConfig);

            gameObject.SetActive(false);
        }

        private async UniTask WaitDependenciesInitAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            Debug.Log("Waiting for LocaleManager and ConfigSaveDataManager initialization...");
            await UniTask.WaitUntil(() =>
                LocalizationSettings.InitializationOperation.IsDone &&
                LocaleManager.GetInstance().IsInitialized, cancellationToken: cToken);

            cToken.ThrowIfCancellationRequested();
            Debug.Log("Waiting for ConfigSaveDataManager to load initial data...");
            await UniTask.WaitUntil(() => ConfigSaveDataManager.GetInstance().LoadedInit, cancellationToken: cToken);
        }
    }
}
