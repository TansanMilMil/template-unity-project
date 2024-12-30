using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class KeyBindForDesktop : IKeyBind
    {
        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleTalk()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Talk,
                    KeyCode.X,
                    shouldDisplayKeySprite: false
                ),
                // サブでマウスのクリックも割り当てる
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Talk,
                    KeyCode.Mouse1,
                    priority: 90,
                    InputKeyBindConditions.OnlyMouse
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Talk,
                    KeyCode.JoystickButton0,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }

        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleDecide()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Decide,
                    KeyCode.Mouse0
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Decide,
                    KeyCode.Space,
                    priority: 90,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Decide,
                    KeyCode.JoystickButton0,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                ),
            };
        }

        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleCancel()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Cancel,
                    KeyCode.X,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Cancel,
                    KeyCode.JoystickButton1,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }

        IEnumerable<InputKeyBind> IKeyBind.GetKeyRolePause()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Pause,
                    KeyCode.Escape,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Pause,
                    KeyCode.JoystickButton7,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }

        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleOptionA()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.OptionA,
                    KeyCode.E,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.OptionA,
                    KeyCode.JoystickButton3,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }

        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleOptionB()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.OptionB,
                    KeyCode.Q,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.OptionB,
                    KeyCode.JoystickButton2,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }

        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleSkip()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Skip,
                    KeyCode.F,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Desktop,
                    KeyRole.Skip,
                    KeyCode.JoystickButton6,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }
    }
}