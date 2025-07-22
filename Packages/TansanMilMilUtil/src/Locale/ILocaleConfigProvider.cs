namespace TansanMilMil.Util
{
    public interface ILocaleConfigProvider
    {
        void LoadConfig();
        string GetStoredCultureInfoName();
        void SaveCultureInfoName(string cultureInfoName);
    }
}