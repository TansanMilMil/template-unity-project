namespace TansanMilMil.Util
{
    public class GamePauseKeyRegistory : Singleton<GamePauseKeyRegistory>, IGamePauseKeyRegistory
    {
        private IGamePauseKey gamePauseKey;

        public void Initialize(IGamePauseKey gamePauseKey)
        {
            this.gamePauseKey = gamePauseKey;
        }

        public IGamePauseKey GetGamePauseKey()
        {
            if (gamePauseKey == null)
            {
                throw new System.InvalidOperationException("GamePauseKeyRegistory is not initialized. Call Initialize() before using GetGamePauseKey().");
            }

            return gamePauseKey;
        }
    }
}
