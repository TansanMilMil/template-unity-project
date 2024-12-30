using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class KeyBindForHandHeld : IKeyBind
    {
        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleTalk()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Handheld,
                    KeyRole.Talk,
                    KeyCode.Mouse1,
                    shouldDisplayKeyNameText: false,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Handheld,
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
                    DeviceType.Handheld,
                    KeyRole.Decide,
                    KeyCode.Mouse0,
                    shouldDisplayKeyNameText: false,
                    shouldDisplayKeySprite: false
                ),
                new InputKeyBind
                (
                    DeviceType.Handheld,
                    KeyRole.Decide,
                    KeyCode.JoystickButton0,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }

        IEnumerable<InputKeyBind> IKeyBind.GetKeyRoleCancel()
        {
            return new List<InputKeyBind>
            {
                new InputKeyBind
                (
                    DeviceType.Handheld,
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
                    DeviceType.Handheld,
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
                    DeviceType.Handheld,
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
                    DeviceType.Handheld,
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
                    DeviceType.Handheld,
                    KeyRole.Skip,
                    KeyCode.JoystickButton6,
                    priority: 80,
                    shouldDisplayKeyNameText: false
                )
            };
        }
    }
}