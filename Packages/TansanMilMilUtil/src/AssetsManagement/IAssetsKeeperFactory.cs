namespace TansanMilMil.Util
{
    public interface IAssetsKeeperFactory
    {
        AssetsKeeper<T> Create<T>(bool autoRelease = false, int autoReleaseOldAssets = 5) where T : UnityEngine.Object;
    }
}