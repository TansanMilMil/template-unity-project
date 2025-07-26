namespace TansanMilMil.Util
{
    public interface IPlayerConfigManager
    {
        bool LoadedInit { get; }
        PlayerConfig GetConfig();
        void SetConfig(PlayerConfig config);
    }
}