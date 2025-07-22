namespace TansanMilMil.Util
{
    public class AssetsTypeSettings
    {
        private static AssetsType assetsType = AssetsType.Resources;

        public enum AssetsType
        {
            Resources = 1,
            Addressables = 2,
        }

        public static AssetsType GetAssetsType()
        {
            return assetsType;
        }

        public static AssetsKeeper<T> NewAssetsKeeper<T>(bool autoRelease = false, IAddressablesWrapper<T> addressablesWrapper = null) where T : UnityEngine.Object
        {
            switch (assetsType)
            {
                case AssetsType.Resources:
                    return new ResourcesKeeper<T>();
                case AssetsType.Addressables:
                    return new AddressablesKeeper<T>(autoRelease, addressablesWrapper);
                default:
                    return null;
            }
        }
    }
}
