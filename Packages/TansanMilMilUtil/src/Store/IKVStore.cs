namespace TansanMilMil.Util
{
    public interface IKVStore
    {
        string Select(string key);
        void Upsert(string key, string data);
        void Delete(string key);
        void DeleteAll();
        bool HasData(string key);
    }
}
