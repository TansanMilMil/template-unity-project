using UnityEngine.Localization;

namespace TansanMilMil.Util
{
    public class LocaleSettings
    {
        private const bool CanUseLocalization = false;
        public readonly static Locale DefaultLocale = AvailableLocales.JAJP;
        public static MessageTextReplacer MessageTextReplacer = new MessageTextReplacer(new MessageTextReplacerLogic());
        public static bool CanUseLocale => CanUseLocalization;
    }
}