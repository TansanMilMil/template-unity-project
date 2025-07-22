using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TansanMilMil.Util
{
    internal class LocaleService : ILocaleService
    {
        private readonly ILocaleConfigProvider configProvider;
        private readonly ILocaleRegistry localeRegistry;
        private bool isInitialized = false;
        private MessageTextReplacer messageTextReplacer;
        private readonly IReadOnlyCollection<TextReplaceStrategy> DefaultReplaceStrategies = new ReadOnlyCollection<TextReplaceStrategy>(
            new List<TextReplaceStrategy>()
            {
                new ReplaceBackSlashNToNewLineStrategy()
            });

        public bool IsInitialized => isInitialized;

        public LocaleService(ILocaleConfigProvider configProvider, ILocaleRegistry localeRegistry, IEnumerable<TextReplaceStrategy> textReplaceStrategies = null)
        {
            this.configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            this.localeRegistry = localeRegistry ?? throw new ArgumentNullException(nameof(localeRegistry));

            IEnumerable<TextReplaceStrategy> strategies = textReplaceStrategies ?? DefaultReplaceStrategies;
            messageTextReplacer = new MessageTextReplacer(strategies);
        }

        public async UniTask InitializeAsync(CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            try
            {
                await UniTask.WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone, cancellationToken: cToken);

                cToken.ThrowIfCancellationRequested();
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
            localeRegistry.Initialize();

            var availableLocales = localeRegistry.GetAvailableLocales();
            if (availableLocales.Count == 0)
            {
                Debug.LogWarning("No locales found in LocalizationSettings. Localization may not work properly.");
            }
            else
            {
                Debug.Log($"Initialized with {availableLocales.Count} available locales: {string.Join(", ", availableLocales.Select(l => l.Identifier.CultureInfo.Name))}");
            }
        }

        private void LoadAndApplyStoredLocale()
        {
            string storedCultureName = configProvider.GetStoredCultureInfoName();

            if (!string.IsNullOrEmpty(storedCultureName))
            {
                SetLocale(storedCultureName);
            }
        }

        public async UniTask<string> GetLocalizedStringAsync(LocaleString localeString, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

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
                var entry = await LocalizationSettings.StringDatabase.GetTableEntryAsync(localeString.tableReference.ToString(), localeString.key).WithCancellation(cToken);

                cToken.ThrowIfCancellationRequested();
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

            Locale locale = localeRegistry.GetLocaleBy(cultureInfoName);

            if (locale == null)
            {
                Debug.LogError($"Locale with culture info '{cultureInfoName}' not found. Available locales: {string.Join(", ", GetAvailableLocales())}. Falling back to default locale.");
                locale = localeRegistry.GetDefaultLocale();

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

            string cultureInfoName = locale.Identifier.CultureInfo.Name;
            configProvider.SetCultureInfoName(cultureInfoName);

            Debug.Log($"Locale changed to: {cultureInfoName}");
        }

        public Locale GetCurrentLocale()
        {
            return LocalizationSettings.SelectedLocale;
        }

        public string GetCurrentCultureInfoName()
        {
            Locale currentLocale = GetCurrentLocale();
            return currentLocale?.Identifier.CultureInfo.Name ?? string.Empty;
        }

        public IReadOnlyList<Locale> GetAvailableLocales()
        {
            return localeRegistry.GetAvailableLocales();
        }

        public bool IsLocaleSupported(string cultureInfoName)
        {
            return localeRegistry.IsLocaleSupported(cultureInfoName);
        }
    }
}
