using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class AssetsTypeSettingRegistry : Singleton<AssetsTypeSettingRegistry>, IAssetsTypeSettingRegistry, IRequireInitialize<IAssetsTypeSetting>
    {
        private IAssetsTypeSetting setting;

        public void Initialize(IAssetsTypeSetting setting)
        {
            this.setting = setting;
        }

        public void AssertInitialized()
        {
            if (setting == null)
            {
                throw new InvalidOperationException("AssetsTypeSetting has not been initialized. Please call Initialize() before using this method.");
            }
        }

        public IAssetsTypeSetting GetAssetsTypeSetting()
        {
            AssertInitialized();

            return setting;
        }
    }
}
