using System.Collections.Generic;
using UnityEngine.Localization;

namespace TansanMilMil.Util
{
    public interface ILocaleRegistry
    {
        IReadOnlyList<Locale> GetAvailableLocales();
        Locale GetLocaleBy(string cultureInfoName);
        Locale GetDefaultLocale();
        bool IsLocaleSupported(string cultureInfoName);
        void Initialize();
    }
}
