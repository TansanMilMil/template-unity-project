using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TansanMilMil.Util
{
    public class LocaleService : ILocaleService
    {
        private readonly ILocaleConfigProvider configProvider;
        private bool isInitialized = false;
        private MessageTextReplacer messageTextReplacer;
        private readonly IReadOnlyCollection<TextReplaceStrategy> DefaultReplaceStrategies = new ReadOnlyCollection<TextReplaceStrategy>(
            new List<TextReplaceStrategy>()
            {
                new ReplaceBackSlashNToNewLineStrategy()
            });

        public bool IsInitialized => isInitialized;

        public LocaleService(ILocaleConfigProvider configProvider, IEnumerable<TextReplaceStrategy> textReplaceStrategies = null)
        {
            this.configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));

            IEnumerable<TextReplaceStrategy> strategies = textReplaceStrategies ?? DefaultReplaceStrategies;
            messageTextReplacer = new MessageTextReplacer(strategies);
        }

        public async UniTask InitializeAsync()
        {
            try
            {
                await UniTask.WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone);

                InitializeAvailableLocales();
                LoadAndApplyStoredLocale();

                isInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to initialize LocaleService: {ex.Message}");
                throw;
            }
        }

        private void InitializeAvailableLocales()
        {
            var enLocale = GetLocaleByName(CultureInfoName.EN_US);
            var jpLocale = GetLocaleByName(CultureInfoName.JA_JP);

            if (enLocale == null || jpLocale == null)
            {
                throw new InvalidOperationException("Required locales (en-US, ja-JP) not found in LocalizationSettings");
            }

            AvailableLocales.Initialize(enLocale, jpLocale);
        }

        private void LoadAndApplyStoredLocale()
        {
            configProvider.LoadConfig();
            string storedCultureName = configProvider.GetStoredCultureInfoName();

            if (!string.IsNullOrEmpty(storedCultureName))
            {
                SetLocale(storedCultureName);
            }
        }

        public async UniTask<string> GetLocalizedStringAsync(LocaleString localeString)
        {
            if (localeString == null)
                throw new ArgumentNullException(nameof(localeString));

            if (string.IsNullOrWhiteSpace(localeString.key))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(localeString.tableReference.ToString()))
                throw new ArgumentException("TableReference cannot be null or whitespace", nameof(localeString.tableReference));

            if (!isInitialized)
                throw new InvalidOperationException("LocaleService is not initialized. Call InitializeAsync() first.");

            try
            {
                var entry = await LocalizationSettings.StringDatabase.GetTableEntryAsync(localeString.tableReference.ToString(), localeString.key);

                if (entry.Entry == null)
                {
                    Debug.LogWarning($"Localization entry not found: {localeString.tableReference}.{localeString.key}");
                    return string.Empty;
                }

                string localizedString = entry.Entry.Value;
                localizedString = messageTextReplacer.Replace(localizedString);

                return localizedString;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get localized string for {localeString.tableReference}.{localeString.key}: {ex.Message}");
                return string.Empty;
            }
        }

        public void SetLocale(string cultureInfoName)
        {
            if (string.IsNullOrWhiteSpace(cultureInfoName))
                throw new ArgumentException("CultureInfoName cannot be null or whitespace", nameof(cultureInfoName));

            Locale locale = GetLocaleByName(cultureInfoName);

            if (locale == null)
            {
                Debug.LogError($"Locale with culture info '{cultureInfoName}' not found. Falling back to default locale.");
                locale = LocaleSettings.DefaultLocale;

                if (locale == null)
                {
                    throw new InvalidOperationException("No default locale available");
                }
            }

            SetLocale(locale);
        }

        public void SetLocale(Locale locale)
        {
            if (locale == null)
                throw new ArgumentNullException(nameof(locale));

            LocalizationSettings.SelectedLocale = locale;

            // Save the new locale setting
            var cultureInfoName = locale.Identifier.CultureInfo.Name;
            configProvider.SaveCultureInfoName(cultureInfoName);

            Debug.Log($"Locale changed to: {cultureInfoName}");
        }

        public Locale GetCurrentLocale()
        {
            return LocalizationSettings.SelectedLocale;
        }

        public string GetCurrentCultureInfoName()
        {
            var currentLocale = GetCurrentLocale();
            return currentLocale?.Identifier.CultureInfo.Name ?? string.Empty;
        }

        private Locale GetLocaleByName(string cultureInfoName)
        {
            return LocalizationSettings.AvailableLocales.Locales
                .Find(locale => locale.Identifier.CultureInfo.Name == cultureInfoName);
        }
    }
}
