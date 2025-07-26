using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlayerConfigManager : Singleton<PlayerConfigManager>, IPlayerConfigManager
    {
        private PlayerConfig config = new();
        public bool LoadedInit { get; private set; } = false;

        public PlayerConfig GetConfig()
        {
            if (!LoadedInit)
            {
                throw new System.InvalidOperationException("Config not loaded yet. Call SetConfig() first.");
            }

            return config;
        }

        public void SetConfig(PlayerConfig config)
        {
            this.config = config;
            LoadedInit = true;
        }
    }
}
