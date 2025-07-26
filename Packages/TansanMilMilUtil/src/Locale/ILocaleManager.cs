using System.Threading;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public interface ILocaleManager
    {
        bool IsInitialized { get; }
        void SetLocale(string cultureInfoName);
        UniTask<string> GetLocalizedStringAsync(LocaleString localeString, CancellationToken cToken = default);
    }
}