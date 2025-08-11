using UnityEngine;

namespace TemplateUnityProject
{
    public static class InitializationBootstrapper
    {
        public static void Initialize()
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
        }
    }
}
