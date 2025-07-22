using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class LocaleManager : SingletonMonoBehaviour<LocaleManager>
    {
        private ILocaleService localeService;
        private bool isInitialized = false;

        protected override void OnSingletonAwake()
        {
            InitializeService();
            InitializeAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void InitializeService()
        {
            ILocaleConfigProvider configProvider = new LocaleConfigProvider();
            ILocaleRegistry localeRegistry = new LocaleRegistry();
            localeService = new LocaleService(configProvider, localeRegistry);
        }

        private async UniTask InitializeAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();
            
            try
            {
                await localeService.InitializeAsync(cToken);
                
                cToken.ThrowIfCancellationRequested();
                isInitialized = true;
                gameObject.SetActive(false);
            }
            catch (Exception ex)
            {
                Debug.LogError($"LocaleManager initialization failed: {ex.Message}");
                throw;
            }
        }

        public bool IsInitialized => isInitialized && localeService.IsInitialized;

        public void SetLocale(string cultureInfoName)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("LocaleManager is not initialized.");
            }

            localeService.SetLocale(cultureInfoName);
        }

        public async UniTask<string> GetLocalizedStringAsync(LocaleString localeString, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();
            
            if (!IsInitialized)
            {
                throw new InvalidOperationException("LocaleManager is not initialized.");
            }

            return await localeService.GetLocalizedStringAsync(localeString, cToken);
        }
    }
}
