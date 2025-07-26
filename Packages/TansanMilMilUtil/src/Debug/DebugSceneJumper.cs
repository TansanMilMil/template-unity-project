using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
namespace TansanMilMil.Util
{
    /// <summary>
    /// デバッグ用シーン移動機能
    /// 開発中にシーン間を素早く移動するためのコンポーネント
    /// </summary>
    public class DebugSceneJumper : MonoBehaviour
    {
        [Header("Scene Jump Settings")]
        [SerializeField] private KeyCode toggleKey = KeyCode.F3;
        [SerializeField] private List<string> sceneNames = new List<string>();

        [Header("UI Settings")]
        [SerializeField] private bool showOnGUI = true;
        [SerializeField] private Vector2 windowPosition = new Vector2(10, 10);
        [SerializeField] private Vector2 windowSize = new Vector2(200, 300);

        private bool isWindowOpen = false;
        private Vector2 scrollPosition = Vector2.zero;

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
        }

        private void OnGUI()
        {
            if (!showOnGUI || !isWindowOpen) return;

            GUI.Window(0, new Rect(windowPosition.x, windowPosition.y, windowSize.x, windowSize.y),
                DrawSceneJumperWindow, "Scene Jumper");
        }

        private void DrawSceneJumperWindow(int windowID)
        {
            GUILayout.BeginVertical();

            GUILayout.Label($"Current Scene: {SceneManager.GetActiveScene().name}");
            GUILayout.Space(10);

            if (GUILayout.Button("Reload Current Scene"))
            {
                ReloadCurrentScene();
            }

            GUILayout.Space(10);
            GUILayout.Label("Jump to Scene:");

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (string sceneName in sceneNames)
            {
                if (GUILayout.Button(sceneName))
                {
                    JumpToScene(sceneName);
                }
            }

            GUILayout.EndScrollView();

            GUILayout.Space(10);

            if (GUILayout.Button("Auto-detect Scenes"))
            {
                AutoDetectScenes();
            }

            if (GUILayout.Button("Close"))
            {
                isWindowOpen = false;
            }

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private void JumpToScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("Scene name is empty");
                return;
            }

            try
            {
                SceneManager.LoadScene(sceneName);
                Debug.Log($"Jumped to scene: {sceneName}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load scene '{sceneName}': {e.Message}");
            }
        }

        private void ReloadCurrentScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
            Debug.Log($"Reloaded scene: {currentSceneName}");
        }

        private void AutoDetectScenes()
        {
            sceneNames.Clear();

            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                if (!string.IsNullOrEmpty(scenePath))
                {
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                    sceneNames.Add(sceneName);
                }
            }

            Debug.Log($"Auto-detected {sceneNames.Count} scenes");
        }

        [ContextMenu("Auto-detect Scenes")]
        private void AutoDetectScenesContextMenu()
        {
            AutoDetectScenes();
        }
    }
}
#endif
