using UnityEngine;

namespace TansanMilMil.Util
{
    public class PlayerPrefsStore : IKVStore
    {
        public string Select(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void Upsert(string key, string data)
        {
            PlayerPrefs.SetString(key, data.ToString());
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
    }
}
