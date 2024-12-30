using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlayerConfigManager
    {
        private static readonly PlayerConfigManager Instance = new();

        private PlayerConfig config = new();

        private PlayerConfigManager() { }

        public static PlayerConfigManager GetInstance()
        {
            return Instance;
        }

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