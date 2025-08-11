using System.Collections.Generic;
using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    public static class FrameRateManagerInitializer
    {
        public static void Initialize()
        {
            var frameRateConfig = CreatePlatformFrameRateConfig();
            FrameRateManager.GetInstance().Initialize(frameRateConfig);

            Debug.Log("FrameRateManager initialized with project-specific configuration");
        }

        private static PlatformFrameRateConfig CreatePlatformFrameRateConfig()
        {
            var configObject = new GameObject("TempFrameRateConfig");
            var config = configObject.AddComponent<PlatformFrameRateConfig>();

            var frameRateItems = new List<PlatformFrameRateItem>
            {
                new PlatformFrameRateItem(RuntimePlatform.WindowsPlayer, 60),
                new PlatformFrameRateItem(RuntimePlatform.WindowsEditor, 60),
                new PlatformFrameRateItem(RuntimePlatform.OSXPlayer, 60),
                new PlatformFrameRateItem(RuntimePlatform.OSXEditor, 60),
                new PlatformFrameRateItem(RuntimePlatform.Android, 30),
                new PlatformFrameRateItem(RuntimePlatform.IPhonePlayer, 30),
                new PlatformFrameRateItem(RuntimePlatform.WebGLPlayer, 60)
            };

            var platformFrameRatesField = typeof(PlatformFrameRateConfig)
                .GetField("platformFrameRates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            platformFrameRatesField?.SetValue(config, frameRateItems);

            Object.DontDestroyOnLoad(configObject);
            return config;
        }
    }
}
