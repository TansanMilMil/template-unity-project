using UnityEngine;

namespace TansanMilMil.Util
{
    public class InputKeys : Singleton<InputKeys>, IInputKeys
    {
        private readonly InputKeyBinds inputKeyBinds = new InputKeyBinds();

        /// <summary>
        /// デフォルトのキーバインドの設定が完了したらtrueを返す。
        /// </summary>
        public bool IsKeyBindsInitialized()
        {
            return inputKeyBinds.initialized;
        }

        public bool AnyInputGetKeyDown(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All)
        {
            return inputKeyBinds.AnyInputGetKeyDown(keyRole, conditions);
        }

        public bool AnyInputGetKey(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All)
        {
            return inputKeyBinds.AnyInputGetKey(keyRole, conditions);
        }

        public string GetSingleKeyNameForUI(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All)
        {
            return inputKeyBinds.GetSingleKeyNameForUI(keyRole, conditions);
        }

        public string GetSingleKeySpritePathForUI(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All)
        {
            return inputKeyBinds.GetSingleKeySpritePathForUI(keyRole, conditions);
        }

        public float GetAxisHorizontal()
        {
            return Input.GetAxis("Horizontal");
        }

        public float GetAxisVertical()
        {
            return Input.GetAxis("Vertical");
        }

        public void InitKeyBinds()
        {
            inputKeyBinds.InitKeyBinds();
        }
    }
}