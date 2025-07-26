namespace TansanMilMil.Util
{
    public interface IAssetsTypeSettingRegistry
    {
        void Register(IAssetsTypeSetting setting);
        IAssetsTypeSetting GetAssetsTypeSetting();
    }
}