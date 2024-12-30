using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public class AddressablesKeeper<T>
    {
        private readonly IAddressablesWrapper<T> addressablesWrapper;
        private List<AddressablesKeeperItem<T>> caches = new List<AddressablesKeeperItem<T>>();
        /// <summary>
        /// trueにするとAssetをLoadした際に古いcacheをReleaseする
        /// </summary>
        private bool autoRelease;
        private const int AutoReleaseOldAssets = 5;

        public AddressablesKeeper(bool autoRelease = false, IAddressablesWrapper<T> addressablesWrapper = null)
        {
            this.autoRelease = autoRelease;
            if (addressablesWrapper == null)
            {
                this.addressablesWrapper = AddressablesWrapper<T>.GetInstance();
            }
            else
            {
                this.addressablesWrapper = addressablesWrapper;
            }
        }

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
                    AsyncOperationHandle<T> handle = addressablesWrapper.LoadAssetAsync(pathName);
                    T asset = await addressablesWrapper.AwaitHandle(handle);
                    caches.Insert(0, new AddressablesKeeperItem<T>(pathName, asset, handle));
                    return asset;
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

        public void ReleaseAsset(string pathName)
        {
            var cache = caches.Find(c => c.pathName == pathName);
            addressablesWrapper.Release(cache.handle);
            caches.Remove(cache);
        }

        public void ReleaseAllAssets()
        {
            foreach (AddressablesKeeperItem<T> cache in caches)
            {
                addressablesWrapper.Release(cache.handle);
            }
            caches.Clear();
        }

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
                addressablesWrapper.Release(caches[caches.Count - 1].handle);
                caches.RemoveAt(caches.Count - 1);
            }
        }
    }
}
