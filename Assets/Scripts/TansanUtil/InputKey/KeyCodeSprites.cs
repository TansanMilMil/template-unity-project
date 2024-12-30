using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public static class KeyCodeSprites
    {
        private static readonly Dictionary<KeyCode, string> sprites = new Dictionary<KeyCode, string>()
    {
        { KeyCode.Mouse0, "Assets/Images/uiUtils/ui_left_click_icon.png" },
        { KeyCode.Mouse1, "Assets/Images/uiUtils/ui_right_click_icon.png" },
    };

        public static string GetSpritePathByKeyCode(KeyCode code)
        {
            if (!sprites.ContainsKey(code))
            {
                return null;
            }

            return sprites[code];
        }
    }
}