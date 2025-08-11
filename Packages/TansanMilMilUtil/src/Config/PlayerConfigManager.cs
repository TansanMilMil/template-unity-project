using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlayerConfigManager : Singleton<PlayerConfigManager>, IPlayerConfigManager
    {
        private PlayerConfig config = new PlayerConfig();

        public PlayerConfig GetConfig()
        {
            if (config == null)
            {
                throw new System.InvalidOperationException("Config not loaded yet. Call SetConfig() first.");
            }

            return config;
        }

        public void SetConfig(PlayerConfig config)
        {
            this.config = config;
        }
    }
}
