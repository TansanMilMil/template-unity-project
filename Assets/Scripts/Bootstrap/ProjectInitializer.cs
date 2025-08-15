using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    [DefaultExecutionOrder(-1000)]
    public class ProjectInitializer : SingletonMonoBehaviour<ProjectInitializer>
    {
        protected override void OnSingletonStart()
        {
            Debug.Log("Starting project initialization... -------------------------");

            ScreenResolutionManagerInitializer.Initialize();
            FrameRateManagerInitializer.Initialize();
            BgmFactoryInitializer.Initialize();
            ConfigSaveDataStoreRegistryInitializer.Initialize();
            DefaultTextReplaceStrategyInitializer.Initialize();
            AssetsTypeSettingRegistryInitializer.Initialize();
            GamePauseKeyRegistoryInitializer.Initialize();

            Debug.Log("Project initialization completed -------------------------");

            Destroy(gameObject);
        }
    }
}
