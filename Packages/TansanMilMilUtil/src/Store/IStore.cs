using System;

namespace TansanMilMil.Util
{
    public interface IStore<TKey, TValue>
    {
        void CreateStoreConnection();
        void CloseStoreConnection();
        TValue Select(TKey key);
        void Insert(TKey key, TValue data);
        void Update(TKey key, TValue data);
        void Upsert(TKey key, TValue data);
        void Delete(TKey key);
        void DeleteAll();
        bool HasData(TKey key);
        public void Commit();
    }
}
