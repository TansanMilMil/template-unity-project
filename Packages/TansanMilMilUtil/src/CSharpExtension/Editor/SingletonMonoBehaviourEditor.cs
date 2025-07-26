using UnityEngine;
using UnityEditor;

namespace TansanMilMil.Util.Editor
{
    [CustomEditor(typeof(SingletonMonoBehaviour<>), true)]
    public class SingletonMonoBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // 目立つヘッダーボックスを表示
            GUIStyle headerStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeColorTexture(new Color(1f, 0.6f, 0f, 0.3f)) }, // オレンジ背景
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            EditorGUILayout.BeginVertical(headerStyle);
            GUILayout.Space(5);

            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                normal = { textColor = new Color(1f, 0.4f, 0f) }, // オレンジテキスト
                alignment = TextAnchor.MiddleCenter
            };

            EditorGUILayout.LabelField("⚡ SINGLETON COMPONENT ⚡", labelStyle);
            EditorGUILayout.LabelField($"Type: {target.GetType().Name}", EditorStyles.centeredGreyMiniLabel);

            GUILayout.Space(5);
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // 通常のインスペクタ表示
            DrawDefaultInspector();
        }

        private Texture2D MakeColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}
