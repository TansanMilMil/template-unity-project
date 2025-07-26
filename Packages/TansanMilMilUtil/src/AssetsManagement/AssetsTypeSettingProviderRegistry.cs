namespace TansanMilMil.Util
{
    public class AssetsTypeSettingRegistry : Singleton<AssetsTypeSettingRegistry>, IAssetsTypeSettingRegistry
    {
        private IAssetsTypeSetting setting;

        public void Register(IAssetsTypeSetting setting)
        {
            this.setting = setting;
        }

        public IAssetsTypeSetting GetAssetsTypeSetting()
        {
            if (setting == null)
            {
                throw new System.InvalidOperationException("AssetsTypeSetting has not been registered. Please call Register() first.");
            }

            return setting;
        }
    }
}
