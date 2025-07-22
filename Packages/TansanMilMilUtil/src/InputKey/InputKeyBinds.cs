using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class InputKeyBinds
    {
        private List<InputKeyBind> inputKeyBinds = new List<InputKeyBind>();

        public bool initialized { get; private set; } = false;
        /// <returns>priorityの降順で<see cref="InputKeyBind"/>のReadOnlyCollectionを返す</returns>
        private IEnumerable<InputKeyBind> GetKeyBindsPriorityOrder(KeyRole keyRole, DeviceType deviceType, InputKeyBindConditions conditions)
        {
            return inputKeyBinds
                .FindAll(x => x.keyRole == keyRole && x.deviceType == deviceType)
                .Where(x => conditions == InputKeyBindConditions.All || x.bindConditions == conditions)
                .OrderByDescending(x => x.priority)
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// UI用のキー名を取得する。<see cref="InputKeyBind.shouldDisplayKeyNameText"/>がfalseの場合は空文字を返す。
        /// 外から使うなら<see cref="InputKeys"/>経由で呼びなさい
        /// </summary>
        public string GetSingleKeyNameForUI(KeyRole keyRole, InputKeyBindConditions conditions)
        {
            IEnumerable<InputKeyBind> inputKeyBinds = GetKeyBindsPriorityOrder(keyRole, UnityEngine.Device.SystemInfo.deviceType, conditions);
            if (inputKeyBinds == null)
            {
                return "";
            }
            // 複数のキー割り当てがあった場合は優先度の高いものを返す
            InputKeyBind keyBind = inputKeyBinds.Where(bind => bind.shouldDisplayKeyNameText).FirstOrDefault();
            if (keyBind == null)
            {
                return "";
            }
            return keyBind.boundKey.ToString();
        }

        /// <summary>
        /// 外から使うなら<see cref="InputKeys"/>経由で呼びなさい
        /// </summary>
        public bool AnyInputGetKeyDown(KeyRole keyRole, InputKeyBindConditions conditions)
        {
            IEnumerable<InputKeyBind> inputKeyBinds = GetKeyBindsPriorityOrder(keyRole, UnityEngine.Device.SystemInfo.deviceType, conditions);
            if (inputKeyBinds == null || inputKeyBinds.Count() == 0)
            {
                Debug.LogError($"No bind for {keyRole} in {UnityEngine.Device.SystemInfo.deviceType} device!");
                return false;
            }

            // 複数の割り当てがあった場合は両方のキーのうちどちらかが押されていればtrueを返す
            return inputKeyBinds.Any(bind => Input.GetKeyDown(bind.boundKey));
        }

        /// <summary>
        /// 外から使うなら<see cref="InputKeys"/>経由で呼びなさい
        /// </summary>
        public bool AnyInputGetKey(KeyRole keyRole, InputKeyBindConditions conditions)
        {
            IEnumerable<InputKeyBind> inputKeyBinds = GetKeyBindsPriorityOrder(keyRole, UnityEngine.Device.SystemInfo.deviceType, conditions);
            if (inputKeyBinds == null || inputKeyBinds.Count() == 0)
            {
                Debug.LogError($"No bind for {keyRole} in {UnityEngine.Device.SystemInfo.deviceType} device!");
                return false;
            }

            // 複数の割り当てがあった場合は両方のキーのうちどちらかが押されていればtrueを返す
            return inputKeyBinds.Any(bind => Input.GetKey(bind.boundKey));
        }

        public string GetSingleKeySpritePathForUI(KeyRole keyRole, InputKeyBindConditions conditions)
        {
            IEnumerable<InputKeyBind> inputKeyBinds = GetKeyBindsPriorityOrder(keyRole, UnityEngine.Device.SystemInfo.deviceType, conditions);
            if (inputKeyBinds == null)
            {
                return null;
            }

            InputKeyBind keyBind = inputKeyBinds.Where(bind => bind.shouldDisplayKeySprite).FirstOrDefault();
            if (keyBind == null)
            {
                return null;
            }
            return KeyCodeSprites.GetSpritePathByKeyCode(keyBind.boundKey);
        }

        private void SetKeyBinds(IEnumerable<InputKeyBind> newKeyBinds)
        {
            foreach (InputKeyBind newKeyBind in newKeyBinds)
            {
                int index = inputKeyBinds.FindIndex(x => IsDuplicated(x, newKeyBind));
                if (index == -1)
                {
                    inputKeyBinds.Add(newKeyBind);
                }
                else
                {
                    inputKeyBinds[index] = newKeyBind;
                }
            }
        }

        private bool IsDuplicated(InputKeyBind KeyBind1, InputKeyBind KeyBind2)
        {
            return KeyBind1.deviceType == KeyBind2.deviceType &&
                KeyBind1.keyRole == KeyBind2.keyRole &&
                KeyBind1.priority == KeyBind2.priority;
        }

        public void InitKeyBinds()
        {
            if (initialized)
            {
                return;
            }

            SetDefaultKeyBinds();
            ValidateKeyBinds();

            initialized = true;
            Debug.Log("InputKeyBinds initialized.");
        }

        private void SetDefaultKeyBinds()
        {
            List<IKeyBind> keyBinds = new List<IKeyBind>
            {
                new KeyBindForDesktop(),
                new KeyBindForHandHeld(),
            };
            foreach (IKeyBind keyBind in keyBinds)
            {
                SetKeyBinds(keyBind.GetKeyRoleTalk());
                SetKeyBinds(keyBind.GetKeyRoleDecide());
                SetKeyBinds(keyBind.GetKeyRoleCancel());
                SetKeyBinds(keyBind.GetKeyRolePause());
                SetKeyBinds(keyBind.GetKeyRoleOptionA());
                SetKeyBinds(keyBind.GetKeyRoleOptionB());
                SetKeyBinds(keyBind.GetKeyRoleSkip());
            }
        }

        private void ValidateKeyBinds()
        {
            // 割り当て設定が漏れてるタイヘンなことになっちゃうので、全ての割り当てが設定されているかチェックしようね
            foreach (KeyRole keyRole in Enum.GetValues(typeof(KeyRole)))
            {
                foreach (DeviceType deviceType in Enum.GetValues(typeof(DeviceType)))
                {
                    if (!SupportedDevices.GetInstance().IsSupported(deviceType))
                    {
                        continue;
                    }

                    IEnumerable<InputKeyBind> binds = GetKeyBindsPriorityOrder(keyRole, deviceType, InputKeyBindConditions.All);
                    if (binds == null || binds.Count() == 0)
                    {
                        throw new Exception($"No bind for {keyRole} in {deviceType} device! Please check current InputKeyBinds!");
                    }
                }
            }
        }
    }
}
