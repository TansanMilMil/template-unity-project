using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization;

namespace TansanMilMil.Util
{
    public interface ILocaleService
    {
        UniTask InitializeAsync();
        UniTask<string> GetLocalizedStringAsync(LocaleString localeString);
        void SetLocale(string cultureInfoName);
        void SetLocale(Locale locale);
        Locale GetCurrentLocale();
        string GetCurrentCultureInfoName();
        IReadOnlyList<Locale> GetAvailableLocales();
        bool IsLocaleSupported(string cultureInfoName);
        bool IsInitialized { get; }
    }
}
