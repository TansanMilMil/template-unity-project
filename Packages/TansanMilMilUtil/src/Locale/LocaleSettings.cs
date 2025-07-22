using UnityEngine.Localization;

namespace TansanMilMil.Util
{
    public static class LocaleSettings
    {
        private const bool CanUseLocalization = true;

        public static Locale DefaultLocale => AvailableLocales.JAJP;
        public static bool CanUseLocale => CanUseLocalization;

        public static string DefaultCultureInfoName => CultureInfoName.JA_JP;
    }
}
