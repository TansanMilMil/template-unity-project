using TansanMilMil.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TemplateUnityProject
{
    public static class GamePauseKeyRegistoryInitializer
    {
        public static void Initialize(InputActionReference pauseAction)
        {
            var gamePauseKey = new InputSystemGamePauseKey(pauseAction);
            GamePauseKeyRegistory.GetInstance().Initialize(gamePauseKey);

            Debug.Log("GamePauseKeyRegistory initialized with Input System pause key (Escape)");
        }

        public class InputSystemGamePauseKey : IGamePauseKey
        {
            private InputActionReference pauseAction;

            public InputSystemGamePauseKey(InputActionReference pauseAction)
            {
                this.pauseAction = pauseAction;
                this.pauseAction.action.Enable();
            }

            ~InputSystemGamePauseKey()
            {
                pauseAction.action.Disable();
            }

            public bool GetKeyDown()
            {
                return pauseAction.action.WasPressedThisFrame();
            }
        }
    }
}
