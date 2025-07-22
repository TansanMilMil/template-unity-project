using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TansanMilMil.Util
{
    public class LocaleRegistry : ILocaleRegistry
    {
        private IEnumerable<Locale> availableLocales;
        private Locale defaultLocale;
        private readonly string defaultCultureInfoName;

        public LocaleRegistry(string defaultCultureInfoName = null)
        {
            string osLocale = System.Globalization.CultureInfo.CurrentCulture.Name;

            this.defaultCultureInfoName = defaultCultureInfoName ?? osLocale;
        }

        public void Initialize()
        {
            if (LocalizationSettings.AvailableLocales?.Locales == null)
            {
                throw new System.InvalidOperationException("LocalizationSettings.AvailableLocales.Locales is null. Cannot initialize locale registry.");
            }

            availableLocales = LocalizationSettings.AvailableLocales.Locales.ToList();

            defaultLocale = GetLocaleBy(defaultCultureInfoName);
            if (defaultLocale == null)
            {
                throw new System.InvalidOperationException($"Default locale '{defaultCultureInfoName}' not found in available locales.");
            }

            Debug.Log($"Locale registry initialized with {availableLocales.Count()} locales. Default: {defaultLocale.Identifier.CultureInfo.Name}");
        }

        public IReadOnlyList<Locale> GetAvailableLocales()
        {
            return availableLocales?.ToList().AsReadOnly() ?? new List<Locale>().AsReadOnly();
        }

        public Locale GetLocaleBy(string cultureInfoName)
        {
            if (string.IsNullOrWhiteSpace(cultureInfoName) || availableLocales == null)
                return null;

            return availableLocales.FirstOrDefault(locale =>
                locale.Identifier.CultureInfo.Name.Equals(cultureInfoName, System.StringComparison.OrdinalIgnoreCase));
        }

        public Locale GetDefaultLocale()
        {
            return defaultLocale;
        }

        public bool IsLocaleSupported(string cultureInfoName)
        {
            return GetLocaleBy(cultureInfoName) != null;
        }
    }
}
