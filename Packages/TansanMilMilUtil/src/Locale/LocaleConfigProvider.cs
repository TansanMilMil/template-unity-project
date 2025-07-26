using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    internal class LocaleConfigProvider : ILocaleConfigProvider
    {
        private IPlayerConfigManager playerConfigManager => PlayerConfigManager.GetInstance();

        public string GetStoredCultureInfoName()
        {
            return playerConfigManager.GetConfig().cultureInfoName;
        }

        public void SetCultureInfoName(string cultureInfoName)
        {
            PlayerConfig config = playerConfigManager.GetConfig();
            config.cultureInfoName = cultureInfoName;
            playerConfigManager.SetConfig(config);
        }
    }
}
