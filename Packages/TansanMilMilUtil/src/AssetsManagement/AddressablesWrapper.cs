using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public class AddressablesWrapper<T> : Singleton<AddressablesWrapper<T>>, IAddressablesWrapper<T>
    {
        public AsyncOperationHandle<T> LoadAssetAsync(string pathName)
        {
            return Addressables.LoadAssetAsync<T>(pathName);
        }

        public void Release(AsyncOperationHandle opHandle)
        {
            Addressables.Release(opHandle);
        }

        public async UniTask<T> AwaitHandle(AsyncOperationHandle<T> handle, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            return await handle.WithCancellation(cToken);
        }
    }

    public class AddressablesWrapper : Singleton<AddressablesWrapper>, IAddressablesDownloader
    {
        public async UniTask DownloadDependenciesAsync(object key, bool autoReleaseHandle = false, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            await Addressables.DownloadDependenciesAsync(key, autoReleaseHandle).WithCancellation(cToken);
        }
    }
}
