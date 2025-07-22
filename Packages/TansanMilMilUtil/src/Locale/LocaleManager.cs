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

        public ILocaleService Service => localeService;

        protected override void OnSingletonAwake()
        {
            InitializeService();
            InitializeAsync().Forget();
        }

        private void InitializeService()
        {
            ILocaleConfigProvider configProvider = new LocaleConfigProvider();
            localeService = new LocaleService(configProvider);
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
    }
}
