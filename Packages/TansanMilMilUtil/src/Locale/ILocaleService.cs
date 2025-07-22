using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization;

namespace TansanMilMil.Util
{
    public interface ILocaleService
    {
        UniTask InitializeAsync(CancellationToken cToken = default);
        UniTask<string> GetLocalizedStringAsync(LocaleString localeString, CancellationToken cToken = default);
        void SetLocale(string cultureInfoName);
        void SetLocale(Locale locale);
        Locale GetCurrentLocale();
        string GetCurrentCultureInfoName();
        IReadOnlyList<Locale> GetAvailableLocales();
        bool IsLocaleSupported(string cultureInfoName);
        bool IsInitialized { get; }
    }
}
