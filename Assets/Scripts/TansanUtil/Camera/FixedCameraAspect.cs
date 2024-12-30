using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace TansanMilMil.Util
{
    /// <summary>
    /// カメラのアスペクト比をリアルタイムに固定する。
    /// このComponentとアタッチしてもCanvas側で左寄せ、右寄せなどのAnchor指定をしているとUIがズレるので注意。
    /// </summary>
    public class FixedCameraAspect : MonoBehaviour
    {
        private float x_aspect = 16.0f;
        private float y_aspect = 9.0f;
        public Camera mainCamera;
        private Vector2 resolution;

        void Awake()
        {
            mainCamera.rect = CalcAspect(x_aspect, y_aspect);
        }

        private void Update()
        {
            // 解像度変更を検知したらカメラのアスペクト比を修正
            if (resolution.x != Screen.width || resolution.y != Screen.height)
            {
                mainCamera.rect = CalcAspect(x_aspect, y_aspect);

                resolution.x = Screen.width;
                resolution.y = Screen.height;
            }
        }

        private Rect CalcAspect(float width, float height)
        {
            float target_aspect = width / height;
            float window_aspect = (float)Screen.width / (float)Screen.height;
            float scale_height = window_aspect / target_aspect;
            Rect rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

            if (1.0f > scale_height)
            {
                rect.x = 0;
                rect.y = (1.0f - scale_height) / 2.0f;
                rect.width = 1.0f;
                rect.height = scale_height;
            }
            else
            {
                float scale_width = 1.0f / scale_height;
                rect.x = (1.0f - scale_width) / 2.0f;
                rect.y = 0.0f;
                rect.width = scale_width;
                rect.height = 1.0f;
            }
            return rect;
        }
    }
}