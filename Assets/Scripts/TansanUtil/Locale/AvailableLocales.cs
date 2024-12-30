using UnityEngine.Localization;

namespace TansanMilMil.Util
{
    public class AvailableLocales
    {
        public static Locale ENUS { get; private set; } = null;
        public static Locale JAJP { get; private set; } = null;

        public static void Initialize(Locale enUS, Locale jaJP)
        {
            ENUS = enUS;
            JAJP = jaJP;
        }
    }
}