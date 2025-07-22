using System;
using System.Runtime.CompilerServices;
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
            InitializeAsync().Forget();
        }

        private void InitializeService()
        {
            ILocaleConfigProvider configProvider = new LocaleConfigProvider();
            ILocaleRegistry localeRegistry = new LocaleRegistry();
            localeService = new LocaleService(configProvider, localeRegistry);
        }

        private async UniTask InitializeAsync()
        {
            try
            {
                await localeService.InitializeAsync();
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
    }
}
