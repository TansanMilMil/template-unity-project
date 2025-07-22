using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public interface ILocaleConfigProvider
    {
        string GetStoredCultureInfoName();
        void SetCultureInfoName(string cultureInfoName);
    }
}
