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
    public class GameLocale : MonoBehaviour
    {
        public static bool Initialized = false;
        // public static Locale ENUS { get; private set; }
        public static Locale JAJP { get; private set; }
        public readonly static Locale DefaultLocale = JAJP;
        private static MessageTextReplacer messageTextReplacer = new MessageTextReplacer(new MessageTextReplacerLogic());

        private void Awake()
        {
            if (Initialized) return;

            InitAsync().Forget();
        }

        private async UniTask InitAsync()
        {
            await UniTask.WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone);
            // ENUS = GetLocale(CultureInfoName.EN_US);
            JAJP = GetLocale(CultureInfoName.JA_JP);

            ConfigSaveDataManager.GetInstance().LoadIfRequired();
            SetLocale(PlayerConfigManager.GetInstance().GetConfig().cultureInfoName);

            PlayerConfig config = PlayerConfigManager.GetInstance().GetConfig();
            config.cultureInfoName = GetCurrentCultureInfoName();
            PlayerConfigManager.GetInstance().SetConfig(config);

            Initialized = true;
            this.gameObject.SetActive(false);
        }

        private static Locale GetLocale(string cultureInfoName)
        {
            return LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.CultureInfo.Name == cultureInfoName);
        }

        private static Locale GetCurrentLocale()
        {
            return LocalizationSettings.SelectedLocale;
        }

        private static void SetLocale(Locale locale)
        {
            LocalizationSettings.SelectedLocale = locale;
        }

        public static void SetLocale(string cultureInfoName)
        {
            Locale locale = GetLocale(cultureInfoName);
            if (locale == null)
            {
                Debug.LogError($"Could not find a locale has cultureInfo << {cultureInfoName} >>. Thus fallback to DefaultLocale {DefaultLocale?.name}.");
                LocalizationSettings.SelectedLocale = DefaultLocale;
                return;
            }
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log($"set locale -> {GetCurrentCultureInfoName()}");
        }

        private static string GetCultureInfoName(Locale locale)
        {
            return locale.Identifier.CultureInfo.Name;
        }

        private static string GetCurrentCultureInfoName()
        {
            return GetCultureInfoName(LocalizationSettings.SelectedLocale);
        }

        /// <summary>
        /// 改行コードを画面上に反映させたい場合は<see cref="GetEntryValueReplacedAsync"/>を使用してください。
        /// </summary>
        public static async UniTask<string> GetEntryValueAsync(string entryName, string tableReference)
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

        public static async UniTask<string> GetEntryValueReplacedAsync(LocaleString localeString)
        {
            if (localeString == null) return "";

            string str = await GetEntryValueAsync(localeString.key, localeString.tableReference.ToString());
            str = localeString.ReplaceTextByRegex(str);
            return messageTextReplacer.Replace(str);
        }

        public static async UniTask<string> GetEntryValueReplacedAsync(string key, string tableReference)
        {
            string str = await GetEntryValueAsync(key, tableReference);
            return messageTextReplacer.Replace(str);
        }
    }
}