using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public class AddressablesKeeper<T> : AssetsKeeper<T> where T : UnityEngine.Object
    {
        private readonly IAddressablesWrapper<T> addressablesWrapper = AddressablesWrapper<T>.GetInstance();

        public AddressablesKeeper(bool autoRelease, int autoReleaseOldAssets)
        {
            this.autoRelease = autoRelease;
            this.autoReleaseOldAssets = autoReleaseOldAssets;
        }

        protected override async UniTask<T> LoadFromAssetAsync(string pathName, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            AsyncOperationHandle<T> handle = addressablesWrapper.LoadAssetAsync(pathName);
            T asset = await addressablesWrapper.AwaitHandle(handle, cToken);
            caches.Insert(0, new AddressablesKeeperItem<T>(pathName, asset, handle));
            return asset;
        }

        public override void ReleaseAsset(string pathName)
        {
            var cache = caches.Find(c => c.pathName == pathName);
            if (cache != null)
            {
                addressablesWrapper.Release((AsyncOperationHandle<T>)cache.handle);
                caches.Remove(cache);
            }
        }

        public override void ReleaseAllAssets()
        {
            foreach (AddressablesKeeperItem<T> cache in caches)
            {
                addressablesWrapper.Release((AsyncOperationHandle<T>)cache.handle);
            }
            caches.Clear();
        }

        public override void ReleaseEndOfAsset(IList<AddressablesKeeperItem<T>> caches)
        {
            if (caches != null && caches.Count > 0)
            {
                int lastIndex = caches.Count - 1;
                addressablesWrapper.Release((AsyncOperationHandle<T>)caches[lastIndex].handle);
                caches.RemoveAt(lastIndex);
            }
        }
    }
}
