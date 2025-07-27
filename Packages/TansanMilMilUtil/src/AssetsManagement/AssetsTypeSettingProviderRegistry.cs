using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireInitializeSingleton]
    public class AssetsTypeSettingRegistry : Singleton<AssetsTypeSettingRegistry>, IAssetsTypeSettingRegistry
    {
        private IAssetsTypeSetting setting;

        public void Initialize(IAssetsTypeSetting setting)
        {
            this.setting = setting;
        }

        public IAssetsTypeSetting GetAssetsTypeSetting()
        {
            if (setting == null)
            {
                Debug.LogError("AssetsTypeSetting has not been registered. Please call Initialize() before using GetAssetsTypeSetting().");
                return null;
            }

            return setting;
        }
    }
}
