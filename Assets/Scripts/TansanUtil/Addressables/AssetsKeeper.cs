using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public abstract class AssetsKeeper<T> where T : UnityEngine.Object
    {
        protected List<AddressablesKeeperItem<T>> caches = new List<AddressablesKeeperItem<T>>();
        /// <summary>
        /// trueにするとAssetをLoadした際に古いcacheをReleaseする
        /// </summary>
        protected bool autoRelease;
        private const int AutoReleaseOldAssets = 5;

        public ReadOnlyCollection<AddressablesKeeperItem<T>> GetCaches()
        {
            return caches.AsReadOnly();
        }

        public async UniTask<T> LoadAssetAsync(string pathName)
        {
            T asset = await LoadAssetOrCacheAsync(pathName);
            AutoRelease();
            return asset;
        }

        private async UniTask<T> LoadAssetOrCacheAsync(string pathName)
        {
            int i = caches.FindIndex(h => h.pathName == pathName);
            if (i == -1)
            {
                try
                {
                    return await LoadFromAssetAsync(pathName);
                }
                catch (InvalidKeyException e)
                {
                    Debug.Log(e.Message);
                    Debug.Log(e.StackTrace);
                    return default(T);
                }
            }
            else
            {
                caches.Insert(0, caches[i]);
                caches.RemoveAt(i + 1);
                return caches[0].asset;
            }
        }

        protected abstract UniTask<T> LoadFromAssetAsync(string pathName);

        public abstract void ReleaseAsset(string pathName);

        public abstract void ReleaseAllAssets();

        private void AutoRelease()
        {
            if (autoRelease)
            {
                ReleaseOldAssets(AutoReleaseOldAssets);
            }
        }

        /// <summary>
        /// 直近でLoadしたAsset以外のAssetのcacheをReleaseする
        /// </summary>
        /// /// <param name="keepLateCache">Releaseせず残すcache数</param>
        private void ReleaseOldAssets(int keepLateCache)
        {
            if (caches.Count <= keepLateCache) return;
            int deleteCount = caches.Count - keepLateCache;

            for (; deleteCount >= 1; deleteCount--)
            {
                ReleaseEndOfAsset(caches);
            }
        }

        public abstract void ReleaseEndOfAsset(List<AddressablesKeeperItem<T>> caches);
    }
}