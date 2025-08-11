using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    public static class AssetsTypeSettingRegistryInitializer
    {
        public static void Initialize()
        {
            var assetTypeSetting = new AssetsTypeSetting();
            AssetsTypeSettingRegistry.GetInstance().Initialize(assetTypeSetting);

            Debug.Log("AssetsTypeSettingRegistry initialized (placeholder)");
        }

        public class AssetsTypeSetting : IAssetsTypeSetting
        {
            public AssetsType GetCurrentAssetsType()
            {
                return AssetsType.Addressables;
            }
        }
    }
}
