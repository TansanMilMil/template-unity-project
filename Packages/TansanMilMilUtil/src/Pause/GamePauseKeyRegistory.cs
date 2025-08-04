using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class GamePauseKeyRegistory : Singleton<GamePauseKeyRegistory>, IGamePauseKeyRegistory, IRequireInitialize<IGamePauseKey>
    {
        private IGamePauseKey gamePauseKey;

        public void Initialize(IGamePauseKey gamePauseKey)
        {
            this.gamePauseKey = gamePauseKey;
        }

        public void AssertInitialized()
        {
            if (gamePauseKey == null)
            {
                throw new InvalidOperationException("GamePauseKey is not initialized. Please call Initialize() before using this method.");
            }
        }

        public IGamePauseKey GetGamePauseKey()
        {
            AssertInitialized();

            return gamePauseKey;
        }
    }
}
