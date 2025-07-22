using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public class LocaleConfigProvider : ILocaleConfigProvider
    {
        public string GetStoredCultureInfoName()
        {
            return PlayerConfigManager.GetInstance().GetConfig().cultureInfoName;
        }

        public void SetCultureInfoName(string cultureInfoName)
        {
            PlayerConfig config = PlayerConfigManager.GetInstance().GetConfig();
            config.cultureInfoName = cultureInfoName;
            PlayerConfigManager.GetInstance().SetConfig(config);
        }
    }
}
