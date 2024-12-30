using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    [Serializable]
    public class InputKeyBind
    {
        public DeviceType deviceType;
        public KeyRole? keyRole;
        public KeyCode boundKey;
        /// <summary>
        /// 数値が大きいほど優先度が高い。基本は100でいい。
        /// 単一のキー割り当てを取得するような処理において、優先度の高いキー割り当てを取得するために使う。
        /// </summary>
        public int priority;
        /// <summary>
        /// 基本は<see cref="InputKeyBindConditions.All"/>を指定する。
        /// 特定の条件下でのみInputKeyBindを取得したい場合はその条件を設定する。
        /// 例えば、マウス専用のUIで<see cref="InputKeyBind"/>を参照したい場合には<see cref="InputKeyBindConditions.OnlyMouse"/> を指定する。
        /// </summary>
        public InputKeyBindConditions bindConditions;
        /// <summary>
        /// joystickのようなユーザーから認識しにくいキー名の時にはfalseを指定してね。
        /// </summary>
        public bool shouldDisplayKeyNameText;
        public bool shouldDisplayKeySprite;

        public InputKeyBind(
            DeviceType deviceType,
            KeyRole keyRole,
            KeyCode boundKey,
            int priority = 100,
            InputKeyBindConditions bindConditions = InputKeyBindConditions.All,
            bool shouldDisplayKeyNameText = true,
            bool shouldDisplayKeySprite = true)
        {
            this.deviceType = deviceType;
            this.keyRole = keyRole;
            this.boundKey = boundKey;
            this.priority = priority;
            this.bindConditions = bindConditions;
            this.shouldDisplayKeyNameText = shouldDisplayKeyNameText;
            this.shouldDisplayKeySprite = shouldDisplayKeySprite;
        }
    }
}