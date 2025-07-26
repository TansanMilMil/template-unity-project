namespace TansanMilMil.Util
{
    public interface IConfigSaveDataStoreRegistry<Tkey, TValue>
    {
        void Initialize(IStore<Tkey, TValue> store, Tkey key);
        IStore<Tkey, TValue> GetConfigSaveDataStore();
        Tkey GetStoreKey();
    }
}
