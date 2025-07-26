using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlayerPrefsStore : IStore<string, string>
    {
        public void CreateStoreConnection()
        {
            // PlayerPrefs does not require a connection setup
        }

        public void CloseStoreConnection()
        {
            // PlayerPrefs does not require a connection teardown
        }

        public string Select(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void Insert(string key, string data)
        {
            throw new System.NotImplementedException("Use Upsert instead of Insert for PlayerPrefsStore.");
        }

        public void Update(string key, string data)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return;
            }
            PlayerPrefs.SetString(key, data);
        }

        public void Upsert(string key, string data)
        {
            PlayerPrefs.SetString(key, data);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public bool HasData(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Commit()
        {
            PlayerPrefs.Save();
        }
    }
}
