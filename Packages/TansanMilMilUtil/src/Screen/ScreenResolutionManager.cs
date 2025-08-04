using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ScreenResolutionManager : Singleton<ScreenResolutionManager>, IScreenResolutionManager, IRequireInitialize<PlatformScreenResolutionConfig>
    {
        private PlatformScreenResolutionConfig config;
        private float InitScreenWidth = -1;
        private float InitScreenHeight = -1;

        public void Initialize(PlatformScreenResolutionConfig resolutionConfig)
        {
            config = resolutionConfig;
        }

        public void AssertInitialized()
        {
            if (config == null)
            {
                throw new InvalidOperationException("ScreenResolutionManager is not initialized. Please call Initialize() before using this method.");
            }
        }

        public void ApplyResolutionForCurrentPlatform()
        {
            AssertInitialized();

            var currentPlatform = UnityEngine.Device.Application.platform;
            ApplyResolutionForPlatform(currentPlatform);
        }

        private void ApplyResolutionForPlatform(RuntimePlatform platform)
        {
            if (!config.HasResolutionForPlatform(platform))
            {
                Debug.LogWarning($"No screen resolution configuration found for platform: {platform}. Using default settings.");
                return;
            }

            var resolutionItem = config.GetResolutionForPlatform(platform);

            if (resolutionItem != null && (resolutionItem.widthRatio != 1f || resolutionItem.heightRatio != 1f))
            {
                ChangeScreenResolution(resolutionItem.widthRatio, resolutionItem.heightRatio);
                Debug.Log($"Applied screen resolution for {platform}: {resolutionItem.widthRatio}x{resolutionItem.heightRatio}");
            }
            else
            {
                Debug.Log($"No screen resolution configuration found for {platform}, using default settings");
            }
        }

        private void ChangeScreenResolution(float screenWidthRatio, float screenHeightRatio)
        {
            // 端末の消費電力を抑えるために解像度を調整する
            InitScreenWidth = Screen.width;
            InitScreenHeight = Screen.height;

            int screenWidth = (int)(Screen.width * screenWidthRatio);
            int screenHeight = (int)(Screen.height * screenHeightRatio);

            Screen.SetResolution(screenWidth, screenHeight, false);

            // Unity Editorでは一度Screenの解像度を変更すると次回デバッグ時も維持されてしまうため、アプリケーション終了時に元の解像度に戻す
            Application.quitting += ResetInitScreenResolution;
            Debug.Log($"SetResolution: {screenWidth}x{screenHeight}");
        }

        private void ResetInitScreenResolution()
        {
            if (InitScreenWidth == -1 || InitScreenHeight == -1)
            {
                Debug.LogError("InitScreenWidth or InitScreenHeight is not set. Cannot reset resolution.");
                return;
            }

            Screen.SetResolution((int)InitScreenWidth, (int)InitScreenHeight, false);
            Debug.Log($"ResetInitScreenResolution: {InitScreenWidth}x{InitScreenHeight}");
        }

        public PlatformScreenResolutionConfig GetConfig()
        {
            AssertInitialized();

            return config;
        }
    }
}
