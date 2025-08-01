using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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
        protected int autoReleaseOldAssets = 5;

        public ReadOnlyCollection<AddressablesKeeperItem<T>> GetCaches()
        {
            return caches.AsReadOnly();
        }

        public async UniTask<T> LoadAssetAsync(string pathName, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            T asset = await LoadAssetOrCacheAsync(pathName, cToken);
            AutoRelease();
            return asset;
        }

        private async UniTask<T> LoadAssetOrCacheAsync(string pathName, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            int i = caches.FindIndex(h => h.pathName == pathName);
            if (i == -1)
            {
                try
                {
                    return await LoadFromAssetAsync(pathName, cToken);
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

        protected abstract UniTask<T> LoadFromAssetAsync(string pathName, CancellationToken cToken = default);

        public abstract void ReleaseAsset(string pathName);

        public abstract void ReleaseAllAssets();

        private void AutoRelease()
        {
            if (autoRelease)
            {
                ReleaseOldAssets(autoReleaseOldAssets);
            }
        }

        /// <summary>
        /// 直近でLoadしたAsset以外のAssetのcacheをReleaseする
        /// </summary>
        /// /// <param name="keepLateCache">Releaseせず残すcache数</param>
        private void ReleaseOldAssets(int keepLateCache)
        {
            if (caches.Count <= keepLateCache)
                return;
            int deleteCount = caches.Count - keepLateCache;

            for (; deleteCount >= 1; deleteCount--)
            {
                ReleaseEndOfAsset(caches);
            }
        }

        public abstract void ReleaseEndOfAsset(IList<AddressablesKeeperItem<T>> caches);
    }
}
