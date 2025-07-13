using UnityEngine;

namespace TansanMilMil.Util
{
    public class ScreenResolution : MonoBehaviour
    {
        private static bool SetCompleted = false;
        private static float InitScreenWidth = -1;
        private static float InitScreenHeight = -1;

        void Start()
        {
            SetResolution();
        }

        private void SetResolution()
        {
            if (SetCompleted) return;

            switch (UnityEngine.Device.Application.platform)
            {
                case RuntimePlatform.Android:
                    ChangeScreenResolution(0.5f, 0.5f);
                    break;
                default:
                    break;
            }
            SetCompleted = true;
        }

        private void ChangeScreenResolution(float screenWidthRatio, float screenHeightRatio)
        {
            // 端末の消費電力を抑えるために解像度を半分にする
            InitScreenWidth = Screen.width;
            InitScreenHeight = Screen.height;

            int screenWidth = (int)(Screen.width * screenWidthRatio);
            int screenHeight = (int)(Screen.height * screenHeightRatio); ;
            Screen.SetResolution(screenWidth, screenHeight, false);

            // Unity Editorでは一度Screenの解像度を変更すると次回デバッグ時も維持されてしまうため、アプリケーション終了時に元の解像度に戻す
            Application.quitting += ResetInitScreenResolution;
            Debug.Log($"SetResolution: {screenWidth}x{screenHeight}");
        }

        private void ResetInitScreenResolution()
        {
            if (InitScreenWidth == -1 || InitScreenHeight == -1)
            {
                throw new System.Exception("InitScreenWidth or InitScreenHeight is not set.");
            }

            Screen.SetResolution((int)InitScreenWidth, (int)InitScreenHeight, false);
            Debug.Log($"ResetInitScreenResolution: {InitScreenWidth}x{InitScreenHeight}");
        }
    }
}