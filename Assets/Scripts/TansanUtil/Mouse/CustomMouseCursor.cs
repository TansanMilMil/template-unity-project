using UnityEngine;

namespace TansanMilMil.Util
{
    public class CustomMouseCursor : MonoBehaviour
    {
        private static bool SetCompleted = false;
        [SerializeField] private Texture2D cursorTexture;
        private readonly Vector2 cursorHotspot = Vector2.zero;

        private void Start()
        {
            SetCusomCursor();
        }

        private void SetCusomCursor()
        {
            if (SetCompleted) return;

            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
            SetCompleted = true;
        }
    }
}