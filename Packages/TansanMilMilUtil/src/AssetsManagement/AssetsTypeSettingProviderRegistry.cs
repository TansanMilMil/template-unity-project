namespace TansanMilMil.Util
{
    public static class AssetsTypeSettingRegistry
    {
        private static IAssetsTypeSetting setting;

        public static void Register(IAssetsTypeSetting setting)
        {
            AssetsTypeSettingRegistry.setting = setting;
        }

        public static IAssetsTypeSetting GetAssetsTypeSetting()
        {
            if (setting == null)
            {
                throw new System.InvalidOperationException("AssetsTypeSetting has not been registered. Please call Register() first.");
            }

            return setting;
        }
    }
}
