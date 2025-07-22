namespace TansanMilMil.Util
{
    public class LocaleConfigProvider : ILocaleConfigProvider
    {
        public void LoadConfig()
        {
            ConfigSaveDataManager.GetInstance().LoadIfRequired();
        }

        public string GetStoredCultureInfoName()
        {
            return PlayerConfigManager.GetInstance().GetConfig().cultureInfoName;
        }

        public void SaveCultureInfoName(string cultureInfoName)
        {
            PlayerConfig config = PlayerConfigManager.GetInstance().GetConfig();
            config.cultureInfoName = cultureInfoName;
            PlayerConfigManager.GetInstance().SetConfig(config);
        }
    }
}