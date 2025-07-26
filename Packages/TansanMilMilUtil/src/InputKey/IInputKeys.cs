namespace TansanMilMil.Util
{
    public interface IInputKeys
    {
        bool IsKeyBindsInitialized();
        bool AnyInputGetKeyDown(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All);
        bool AnyInputGetKey(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All);
        string GetSingleKeyNameForUI(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All);
        string GetSingleKeySpritePathForUI(KeyRole keyRole, InputKeyBindConditions conditions = InputKeyBindConditions.All);
        float GetAxisHorizontal();
        float GetAxisVertical();
        void InitKeyBinds();
    }
}