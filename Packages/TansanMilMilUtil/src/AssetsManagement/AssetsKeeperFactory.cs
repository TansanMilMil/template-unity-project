namespace TansanMilMil.Util
{
    public class AssetsKeeperFactory
    {
        private AssetsType assetsType;

        public AssetsKeeperFactory(IAssetsTypeSetting assetsTypeSetting)
        {
            assetsType = assetsTypeSetting.GetCurrentAssetsType();
        }

        public AssetsKeeper<T> Create<T>(bool autoRelease = false, int autoReleaseOldAssets = 5) where T : UnityEngine.Object
        {
            switch (assetsType)
            {
                case AssetsType.Addressables:
                    return new AddressablesKeeper<T>(autoRelease, autoReleaseOldAssets);
                case AssetsType.Resources:
                    return new ResourcesKeeper<T>(autoRelease, autoReleaseOldAssets);
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
    }
}
