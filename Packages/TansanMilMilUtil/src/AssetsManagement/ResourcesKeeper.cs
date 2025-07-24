using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ResourcesKeeper<T> : AssetsKeeper<T> where T : UnityEngine.Object
    {
        public ResourcesKeeper(bool autoRelease, int autoReleaseOldAssets)
        {
            this.autoRelease = autoRelease;
            this.autoReleaseOldAssets = autoReleaseOldAssets;
        }

        protected override async UniTask<T> LoadFromAssetAsync(string pathName, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            T asset = Resources.Load<T>(pathName);
            caches.Insert(0, new AddressablesKeeperItem<T>(pathName, asset, null));
            await UniTask.CompletedTask;
            return asset;
        }

        public override void ReleaseAsset(string pathName)
        {
            var cache = caches.Find(c => c.pathName == pathName);
            if (cache != null)
            {
                caches.Remove(cache);
            }
        }

        public override void ReleaseAllAssets()
        {
            caches.Clear();
        }

        public override void ReleaseEndOfAsset(IList<AddressablesKeeperItem<T>> caches)
        {
            if (caches.Count > 0)
            {
                caches.RemoveAt(caches.Count - 1);
            }
        }
    }
}
