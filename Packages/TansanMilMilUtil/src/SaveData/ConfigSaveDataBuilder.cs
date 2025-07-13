using TansanMilMil.Util;

namespace TansanMilMil.Util
{
    public class ConfigSaveDataBuilder : IBuilder<ConfigSaveData>
    {
        private ConfigSaveData saveData = new ConfigSaveData();

        public ConfigSaveDataBuilder playerConfig(PlayerConfig playerConfig)
        {
            this.saveData.playerConfig = playerConfig;
            return this;
        }

        public ConfigSaveData Build()
        {
            return saveData;
        }
    }
}