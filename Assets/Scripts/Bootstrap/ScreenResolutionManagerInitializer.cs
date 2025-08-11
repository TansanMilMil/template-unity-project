using System.Collections.Generic;
using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    public static class ScreenResolutionManagerInitializer
    {
        public static void Initialize()
        {
            var resolutionItems = new List<PlatformScreenResolutionItem>
            {
                new PlatformScreenResolutionItem(RuntimePlatform.WindowsPlayer, 1.0f, 1.0f),
                new PlatformScreenResolutionItem(RuntimePlatform.WindowsEditor, 1.0f, 1.0f),
                new PlatformScreenResolutionItem(RuntimePlatform.OSXPlayer, 1.0f, 1.0f),
                new PlatformScreenResolutionItem(RuntimePlatform.OSXEditor, 1.0f, 1.0f),
                new PlatformScreenResolutionItem(RuntimePlatform.Android, 0.8f, 0.8f),
                new PlatformScreenResolutionItem(RuntimePlatform.IPhonePlayer, 0.8f, 0.8f),
                new PlatformScreenResolutionItem(RuntimePlatform.WebGLPlayer, 1.0f, 1.0f)
            };

            var config = new PlatformScreenResolutionConfig(resolutionItems);
            ScreenResolutionManager.GetInstance().Initialize(config);

            Debug.Log("ScreenResolutionManager initialized with project-specific configuration");
        }
    }
}
