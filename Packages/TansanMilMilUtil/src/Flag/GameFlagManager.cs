using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// ゲーム内フラグを管理するシングルトンクラス
    /// key-value形式でフラグを保持し、アセンブリ外から設定可能
    /// </summary>
    public class GameFlagManager : Singleton<GameFlagManager>
    {
        private Dictionary<string, string> flags = new Dictionary<string, string>();

        /// <summary>
        /// フラグを設定
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="value">フラグの値</param>
        public void SetFlag(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("GameFlagManager: Key cannot be null or empty");
                return;
            }

            flags[key] = value ?? string.Empty;
            Debug.Log($"GameFlagManager: Set flag '{key}' = '{value}'");
        }

        /// <summary>
        /// フラグの値を取得
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="defaultValue">キーが存在しない場合のデフォルト値</param>
        /// <returns>フラグの値</returns>
        public string GetFlag(string key, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("GameFlagManager: Key cannot be null or empty");
                return defaultValue;
            }

            return flags.TryGetValue(key, out string value) ? value : defaultValue;
        }

        /// <summary>
        /// フラグが存在するかチェック
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <returns>フラグが存在する場合true</returns>
        public bool HasFlag(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            return flags.ContainsKey(key);
        }

        /// <summary>
        /// フラグを削除
        /// </summary>
        /// <param name="key">削除するフラグのキー</param>
        /// <returns>削除に成功した場合true</returns>
        public bool RemoveFlag(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("GameFlagManager: Key cannot be null or empty");
                return false;
            }

            bool removed = flags.Remove(key);
            if (removed)
            {
                Debug.Log($"GameFlagManager: Removed flag '{key}'");
            }
            return removed;
        }

        /// <summary>
        /// 全てのフラグをクリア
        /// </summary>
        public void ClearAllFlags()
        {
            int count = flags.Count;
            flags.Clear();
            Debug.Log($"GameFlagManager: Cleared {count} flags");
        }

        /// <summary>
        /// ブール値としてフラグを設定
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="value">ブール値</param>
        public void SetBoolFlag(string key, bool value)
        {
            SetFlag(key, value.ToString());
        }

        /// <summary>
        /// ブール値としてフラグを取得
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>ブール値</returns>
        public bool GetBoolFlag(string key, bool defaultValue = false)
        {
            string value = GetFlag(key, defaultValue.ToString());
            return bool.TryParse(value, out bool result) ? result : defaultValue;
        }

        /// <summary>
        /// 整数値としてフラグを設定
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="value">整数値</param>
        public void SetIntFlag(string key, int value)
        {
            SetFlag(key, value.ToString());
        }

        /// <summary>
        /// 整数値としてフラグを取得
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>整数値</returns>
        public int GetIntFlag(string key, int defaultValue = 0)
        {
            string value = GetFlag(key, defaultValue.ToString());
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        /// <summary>
        /// 浮動小数点値としてフラグを設定
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="value">浮動小数点値</param>
        public void SetFloatFlag(string key, float value)
        {
            SetFlag(key, value.ToString());
        }

        /// <summary>
        /// 浮動小数点値としてフラグを取得
        /// </summary>
        /// <param name="key">フラグのキー</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns>浮動小数点値</returns>
        public float GetFloatFlag(string key, float defaultValue = 0f)
        {
            string value = GetFlag(key, defaultValue.ToString());
            return float.TryParse(value, out float result) ? result : defaultValue;
        }

        /// <summary>
        /// 複数のフラグを一括設定
        /// </summary>
        /// <param name="flagsToSet">設定するフラグのDictionary</param>
        public void SetFlags(Dictionary<string, string> flagsToSet)
        {
            if (flagsToSet == null)
            {
                Debug.LogWarning("GameFlagManager: Cannot set null flags dictionary");
                return;
            }

            foreach (var kvp in flagsToSet)
            {
                SetFlag(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// 全てのフラグを取得
        /// </summary>
        /// <returns>全フラグのコピー</returns>
        public Dictionary<string, string> GetAllFlags()
        {
            return new Dictionary<string, string>(flags);
        }

        /// <summary>
        /// 指定されたキーのプレフィックスを持つフラグを取得
        /// </summary>
        /// <param name="prefix">プレフィックス</param>
        /// <returns>該当するフラグのDictionary</returns>
        public Dictionary<string, string> GetFlagsWithPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return new Dictionary<string, string>();
            }

            return flags.Where(kvp => kvp.Key.StartsWith(prefix))
                       .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// フラグの内容をログに出力（デバッグ用）
        /// </summary>
        public void DebugPrintAllFlags()
        {
            Debug.Log($"GameFlagManager: Total flags: {flags.Count}");
            foreach (var kvp in flags)
            {
                Debug.Log($"  '{kvp.Key}' = '{kvp.Value}'");
            }
        }
    }
}
