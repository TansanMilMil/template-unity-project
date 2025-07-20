using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlayerConfigManager : Singleton<PlayerConfigManager>
    {
        private PlayerConfig config = new();

        public PlayerConfig GetConfig()
        {
            return this.config;
        }

        public void SetConfig(PlayerConfig config)
        {
            this.config = config;
        }
    }
}