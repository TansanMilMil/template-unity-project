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
        bool IsInitialized { get; }
    }
}
