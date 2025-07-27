namespace TansanMilMil.Util
{
    public interface IAssetsTypeSettingRegistry
    {
        void Initialize(IAssetsTypeSetting setting);
        IAssetsTypeSetting GetAssetsTypeSetting();
    }
}
