using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public class AddressablesKeeper<T> : AssetsKeeper<T> where T : UnityEngine.Object
    {
        private readonly IAddressablesWrapper<T> addressablesWrapper;

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
        protected override async UniTask<T> LoadFromAssetAsync(string pathName)
        {
            AsyncOperationHandle<T> handle = addressablesWrapper.LoadAssetAsync(pathName);
            T asset = await addressablesWrapper.AwaitHandle(handle);
            caches.Insert(0, new AddressablesKeeperItem<T>(pathName, asset, handle));
            return asset;
        }

        public override void ReleaseAsset(string pathName)
        {
            var cache = caches.Find(c => c.pathName == pathName);
            addressablesWrapper.Release((AsyncOperationHandle<T>)cache.handle);
            caches.Remove(cache);
        }

        public override void ReleaseAllAssets()
        {
            foreach (AddressablesKeeperItem<T> cache in caches)
            {
                addressablesWrapper.Release((AsyncOperationHandle<T>)cache.handle);
            }
            caches.Clear();
        }

        public override void ReleaseEndOfAsset(List<AddressablesKeeperItem<T>> caches)
        {
            addressablesWrapper.Release((AsyncOperationHandle<T>)caches[caches.Count - 1].handle);
            caches.RemoveAt(caches.Count - 1);
        }
    }
}
