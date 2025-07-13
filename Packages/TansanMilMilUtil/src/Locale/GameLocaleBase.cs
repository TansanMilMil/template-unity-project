using Cysharp.Threading.Tasks;
using TansanMilMil.Util;
using R3;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System;
using Unity.VisualScripting;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public abstract class GameLocaleBase : MonoBehaviour
    {
        public static bool Initialized = false;
        private static GameLocaleBase instanceCache;

        private void Awake()
        {
            if (Initialized) return;

            InitAsync().Forget();
        }

        public static GameLocaleBase GetInstanceOnCurrentScene()
        {
            if (instanceCache && instanceCache.gameObject)
            {
                return instanceCache;
            }

            instanceCache = FindAnyObjectByType<GameLocaleBase>();
            if (instanceCache == null)
            {
                throw new Exception("GameLocaleBase instance is not found!");
            }
            return instanceCache;
        }

        private async UniTask InitAsync()
        {
            await InitializeAvailableLocalesAsync();

            LoadLocaleFromConfigSaveData();
            SetLocaleFromConfigSaveData();
            SetLocaleToConfig();

            Initialized = true;
            this.gameObject.SetActive(false);
        }

        protected abstract UniTask InitializeAvailableLocalesAsync();

        protected abstract void LoadLocaleFromConfigSaveData();

        protected abstract void SetLocaleFromConfigSaveData();

        protected abstract void SetLocaleToConfig();

        protected abstract Locale GetLocale(string cultureInfoName);

        protected abstract Locale GetCurrentLocale();

        protected abstract void SetLocale(Locale locale);

        public abstract void SetLocale(string cultureInfoName);

        private string GetCultureInfoName(Locale locale)
        {
            return locale.Identifier.CultureInfo.Name;
        }

        protected string GetCurrentCultureInfoName()
        {
            return GetCultureInfoName(LocalizationSettings.SelectedLocale);
        }

        /// <summary>
        /// 改行コードを画面上に反映させたい場合は<see cref="GetEntryValueReplacedAsync"/>を使用してください。
        /// </summary>
        public abstract UniTask<string> GetEntryValueAsync(string entryName, string tableReference);

        public async UniTask<string> GetEntryValueReplacedAsync(LocaleString localeString)
        {
            if (localeString == null) return "";

            string str = await GetEntryValueAsync(localeString.key, localeString.tableReference.ToString());
            str = localeString.ReplaceTextByRegex(str);
            return LocaleSettings.MessageTextReplacer.Replace(str);
        }

        public async UniTask<string> GetEntryValueReplacedAsync(string key, string tableReference)
        {
            string str = await GetEntryValueAsync(key, tableReference);
            return LocaleSettings.MessageTextReplacer.Replace(str);
        }
    }
}