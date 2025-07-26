using UnityEngine;
using UnityEditor;

namespace TansanMilMil.Util.Editor
{
    [InitializeOnLoad]
    public static class SingletonHierarchyIcon
    {
        static SingletonHierarchyIcon()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null)
                return;

            // SingletonMonoBehaviourを継承するコンポーネントがあるかチェック
            var singletonComponent = obj.GetComponent<MonoBehaviour>();
            if (singletonComponent == null)
                return;

            var type = singletonComponent.GetType();
            while (type != null && type != typeof(MonoBehaviour))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SingletonMonoBehaviour<>))
                {
                    // アイコンを表示
                    Rect iconRect = new Rect(selectionRect.xMax - 20, selectionRect.y, 16, 16);

                    GUIContent iconContent = new GUIContent(
                        EditorGUIUtility.IconContent("d_Settings").image,
                        "Singleton Component"
                    );

                    GUI.Label(iconRect, iconContent);

                    // オブジェクト名の色を変更
                    if (Event.current.type == EventType.Repaint)
                    {
                        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
                        {
                            normal = { textColor = new Color(1f, 0.6f, 0f) }
                        };

                        Rect labelRect = new Rect(selectionRect.x + 16, selectionRect.y, selectionRect.width - 36, selectionRect.height);
                        labelStyle.Draw(labelRect, obj.name, false, false, false, false);
                    }
                    break;
                }
                type = type.BaseType;
            }
        }
    }
}
