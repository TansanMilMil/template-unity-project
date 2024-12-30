using System;
using Cysharp.Threading.Tasks;
using TansanMilMil.Util;
using R3;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class GameLocale : GameLocaleBase
    {
        protected override async UniTask InitializeAvailableLocalesAsync()
        {
            await UniTask.WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone);

            AvailableLocales.Initialize(
                GetLocale(CultureInfoName.EN_US),
                GetLocale(CultureInfoName.JA_JP)
            );
        }

        protected override void LoadLocaleFromConfigSaveData()
        {
            ConfigSaveDataManager.GetInstance().LoadIfRequired();
        }

        protected override void SetLocaleFromConfigSaveData()
        {
            SetLocale(PlayerConfigManager.GetInstance().GetConfig().cultureInfoName);
        }

        protected override void SetLocaleToConfig()
        {
            PlayerConfig config = PlayerConfigManager.GetInstance().GetConfig();
            config.cultureInfoName = GetCurrentCultureInfoName();
            PlayerConfigManager.GetInstance().SetConfig(config);
        }

        protected override Locale GetLocale(string cultureInfoName)
        {
            return LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.CultureInfo.Name == cultureInfoName);
        }

        protected override Locale GetCurrentLocale()
        {
            return LocalizationSettings.SelectedLocale;
        }

        protected override void SetLocale(Locale locale)
        {
            LocalizationSettings.SelectedLocale = locale;
        }

        public override void SetLocale(string cultureInfoName)
        {
            Locale locale = GetLocale(cultureInfoName);
            if (locale == null)
            {
                Debug.LogError($"Could not find a locale has cultureInfo << {cultureInfoName} >>. Thus fallback to DefaultLocale {LocaleSettings.DefaultLocale?.name}.");
                LocalizationSettings.SelectedLocale = LocaleSettings.DefaultLocale;
                return;
            }
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log($"set locale -> {GetCurrentCultureInfoName()}");
        }

        /// <summary>
        /// 改行コードを画面上に反映させたい場合は<see cref="GetEntryValueReplacedAsync"/>を使用してください。
        /// </summary>
        public override async UniTask<string> GetEntryValueAsync(string entryName, string tableReference)
        {
            if (string.IsNullOrWhiteSpace(entryName)) return "";
            if (string.IsNullOrWhiteSpace(tableReference)) throw new Exception("tableReference is null or whiteSpace!");

            await UniTask.WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone);

            var e = await LocalizationSettings.StringDatabase.GetTableEntryAsync(tableReference, entryName);
            if (e.Entry == null)
            {
                Debug.Log($"{tableReference}.{entryName} was not found! Thus return empty string.");
                return "";
            }
            return e.Entry.Value;
        }
    }
}