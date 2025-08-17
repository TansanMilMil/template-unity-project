using TansanMilMil.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TemplateUnityProject
{
    [DefaultExecutionOrder(-1000)]
    public class ProjectInitializer : SingletonMonoBehaviour<ProjectInitializer>
    {
        [SerializeField]
        private InputActionReference pauseAction;

        protected override void OnSingletonStart()
        {
            Debug.Log("Starting project initialization... -------------------------");

            ScreenResolutionManagerInitializer.Initialize();
            FrameRateManagerInitializer.Initialize();
            BgmFactoryInitializer.Initialize();
            ConfigSaveDataStoreRegistryInitializer.Initialize();
            DefaultTextReplaceStrategyInitializer.Initialize();
            AssetsTypeSettingRegistryInitializer.Initialize();
            GamePauseKeyRegistoryInitializer.Initialize(pauseAction);

            Debug.Log("Project initialization completed -------------------------");

            Destroy(gameObject);
        }
    }
}
