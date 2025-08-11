using TansanMilMil.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TemplateUnityProject
{
    public static class GamePauseKeyRegistoryInitializer
    {
        public static void Initialize()
        {
            var gamePauseKey = new InputSystemGamePauseKey();
            GamePauseKeyRegistory.GetInstance().Initialize(gamePauseKey);

            Debug.Log("GamePauseKeyRegistory initialized with Input System pause key (Escape)");
        }

        public class InputSystemGamePauseKey : IGamePauseKey
        {
            private InputAction pauseAction;

            public InputSystemGamePauseKey()
            {
                pauseAction = new InputAction("Pause", InputActionType.Button, "<Keyboard>/escape");
                pauseAction.Enable();
            }

            public bool GetKeyDown()
            {
                return pauseAction.WasPressedThisFrame();
            }
        }
    }
}
