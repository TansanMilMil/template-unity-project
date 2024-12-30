using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ResourcesKeeper<T> : AssetsKeeper<T> where T : UnityEngine.Object
    {
        protected override async UniTask<T> LoadFromAssetAsync(string pathName)
        {
            T asset = Resources.Load<T>(pathName);
            caches.Insert(0, new AddressablesKeeperItem<T>(pathName, asset, null));
            await UniTask.CompletedTask;
            return asset;
        }

        public override void ReleaseAsset(string pathName)
        {
            var cache = caches.Find(c => c.pathName == pathName);
            caches.Remove(cache);
        }

        public override void ReleaseAllAssets()
        {
            caches.Clear();
        }

        public override void ReleaseEndOfAsset(List<AddressablesKeeperItem<T>> caches)
        {
            caches.RemoveAt(caches.Count - 1);
        }
    }
}