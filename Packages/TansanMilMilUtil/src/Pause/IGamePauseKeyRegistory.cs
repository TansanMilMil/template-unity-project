namespace TansanMilMil.Util
{
    public interface IGamePauseKeyRegistory
    {
        void Initialize(IGamePauseKey gamePauseKey);
        IGamePauseKey GetGamePauseKey();
    }
}
