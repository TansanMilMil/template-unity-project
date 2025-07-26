using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
namespace TansanMilMil.Util
{
    /// <summary>
    /// デバッグ用フラグ編集機能
    /// GameFlagManagerで管理されているフラグを動的に変更・確認するためのコンポーネント
    /// </summary>
    public class DebugFlagEditor : MonoBehaviour
    {
        [Header("Flag Editor Settings")]
        [SerializeField] private KeyCode toggleKey = KeyCode.F5;

        [Header("UI Settings")]
        [SerializeField] private bool showOnGUI = true;
        [SerializeField] private Vector2 windowPosition = new Vector2(530, 10);
        [SerializeField] private Vector2 windowSize = new Vector2(350, 450);

        private bool isWindowOpen = false;
        private Vector2 scrollPosition = Vector2.zero;

        // フラグ編集用の変数
        private string newFlagKey = "";
        private string newFlagValue = "";
        private string searchFilter = "";
        private Dictionary<string, string> editingFlags = new Dictionary<string, string>();

        private GameFlagManager flagManager => GameFlagManager.GetInstance();

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                ToggleWindow();
            }
        }

        private void ToggleWindow()
        {
            isWindowOpen = !isWindowOpen;
            if (isWindowOpen)
            {
                RefreshEditingFlags();
            }
        }

        private void RefreshEditingFlags()
        {
            editingFlags.Clear();
            var allFlags = flagManager.GetAllFlags();
            foreach (var kvp in allFlags)
            {
                editingFlags[kvp.Key] = kvp.Value;
            }
        }

        private void OnGUI()
        {
            if (!showOnGUI || !isWindowOpen) return;

            GUI.Window(2, new Rect(windowPosition.x, windowPosition.y, windowSize.x, windowSize.y),
                DrawFlagEditorWindow, "Flag Editor");
        }

        private void DrawFlagEditorWindow(int windowID)
        {
            GUILayout.BeginVertical();

            // ヘッダー情報
            GUILayout.Label($"Total Flags: {editingFlags.Count}", GUI.skin.box);
            GUILayout.Space(5);

            // 新しいフラグ追加セクション
            GUILayout.Label("Add New Flag:", GUI.skin.box);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Key:", GUILayout.Width(30));
            newFlagKey = GUILayout.TextField(newFlagKey, GUILayout.Width(100));
            GUILayout.Label("Value:", GUILayout.Width(40));
            newFlagValue = GUILayout.TextField(newFlagValue, GUILayout.Width(100));
            if (GUILayout.Button("Add", GUILayout.Width(40)))
            {
                AddNewFlag();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            // 検索フィルター
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search:", GUILayout.Width(50));
            searchFilter = GUILayout.TextField(searchFilter);
            if (GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                searchFilter = "";
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            // フラグリスト
            GUILayout.Label("Flags:", GUI.skin.box);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

            var filteredFlags = GetFilteredFlags();
            var sortedFlags = filteredFlags.OrderBy(kvp => kvp.Key).ToList();

            foreach (var kvp in sortedFlags)
            {
                DrawFlagEntry(kvp.Key, kvp.Value);
            }

            if (sortedFlags.Count == 0)
            {
                GUILayout.Label("No flags found", GUI.skin.box);
            }

            GUILayout.EndScrollView();

            GUILayout.Space(10);

            // 操作ボタン
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply All"))
            {
                ApplyAllFlags();
            }
            if (GUILayout.Button("Refresh"))
            {
                RefreshEditingFlags();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Print All"))
            {
                flagManager.DebugPrintAllFlags();
            }
            if (GUILayout.Button("Clear All"))
            {
                ClearAllFlags();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            // プリセット操作
            GUILayout.Label("Quick Actions:", GUI.skin.box);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Tutorial Off"))
            {
                SetQuickFlag("tutorial_completed", "true");
            }
            if (GUILayout.Button("Debug Mode"))
            {
                SetQuickFlag("debug_mode", "true");
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (GUILayout.Button("Close"))
            {
                isWindowOpen = false;
            }

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private void DrawFlagEntry(string key, string value)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);

            // キー表示（削除ボタン付き）
            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                RemoveFlag(key);
                return;
            }
            GUILayout.Label(key, GUILayout.Width(120));

            // 値編集
            string newValue = GUILayout.TextField(value, GUILayout.Width(100));
            if (newValue != value)
            {
                editingFlags[key] = newValue;
            }

            // 型別設定ボタン
            if (GUILayout.Button("T", GUILayout.Width(20)))
            {
                editingFlags[key] = "true";
            }
            if (GUILayout.Button("F", GUILayout.Width(20)))
            {
                editingFlags[key] = "false";
            }

            GUILayout.EndHorizontal();
        }

        private Dictionary<string, string> GetFilteredFlags()
        {
            if (string.IsNullOrEmpty(searchFilter))
            {
                return editingFlags;
            }

            return editingFlags.Where(kvp =>
                kvp.Key.ToLower().Contains(searchFilter.ToLower()) ||
                kvp.Value.ToLower().Contains(searchFilter.ToLower())
            ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private void AddNewFlag()
        {
            if (string.IsNullOrEmpty(newFlagKey))
            {
                Debug.LogWarning("DebugFlagEditor: Key cannot be empty");
                return;
            }

            editingFlags[newFlagKey] = newFlagValue;
            flagManager.SetFlag(newFlagKey, newFlagValue);

            Debug.Log($"DebugFlagEditor: Added flag '{newFlagKey}' = '{newFlagValue}'");

            // 入力フィールドをクリア
            newFlagKey = "";
            newFlagValue = "";
        }

        private void RemoveFlag(string key)
        {
            if (editingFlags.ContainsKey(key))
            {
                editingFlags.Remove(key);
                flagManager.RemoveFlag(key);
                Debug.Log($"DebugFlagEditor: Removed flag '{key}'");
            }
        }

        private void ApplyAllFlags()
        {
            foreach (var kvp in editingFlags)
            {
                flagManager.SetFlag(kvp.Key, kvp.Value);
            }
            Debug.Log($"DebugFlagEditor: Applied {editingFlags.Count} flags");
        }

        private void ClearAllFlags()
        {
            if (editingFlags.Count > 0)
            {
                editingFlags.Clear();
                flagManager.ClearAllFlags();
                Debug.Log("DebugFlagEditor: Cleared all flags");
            }
        }

        private void SetQuickFlag(string key, string value)
        {
            editingFlags[key] = value;
            flagManager.SetFlag(key, value);
            Debug.Log($"DebugFlagEditor: Quick set '{key}' = '{value}'");
        }

        [ContextMenu("Refresh Flags")]
        private void RefreshFlagsContextMenu()
        {
            RefreshEditingFlags();
            Debug.Log("DebugFlagEditor: Refreshed flags from GameFlagManager");
        }

        [ContextMenu("Export Flags")]
        private void ExportFlags()
        {
            string export = "Current Flags:\n";
            foreach (var kvp in editingFlags.OrderBy(x => x.Key))
            {
                export += $"  {kvp.Key} = {kvp.Value}\n";
            }
            Debug.Log(export);
        }
    }
}
#endif
