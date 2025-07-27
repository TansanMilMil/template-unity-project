using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireInitializeSingleton]
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
                Debug.LogError("GamePauseKeyRegistory is not initialized. Please call Initialize() before using GetGamePauseKey().");
                return null;
            }

            return gamePauseKey;
        }
    }
}
