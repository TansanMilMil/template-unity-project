namespace TansanMilMil.Util
{
    public interface IPlayerConfigManager
    {
        PlayerConfig GetConfig();
        void SetConfig(PlayerConfig config);
    }
}
