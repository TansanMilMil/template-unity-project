namespace TansanMilMil.Util
{
    public interface IConfigSaveDataStoreRegistry<Tkey, TValue>
    {
        IStore<Tkey, TValue> GetConfigSaveDataStore();
        Tkey GetStoreKey();
    }
}
