using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public interface IAddressablesWrapper<T>
    {
        AsyncOperationHandle<T> LoadAssetAsync(string pathName);
        void Release(AsyncOperationHandle opHandle);
        UniTask<T> AwaitHandle(AsyncOperationHandle<T> handle, CancellationToken cToken = default);
    }

    public interface IAddressablesDownloader
    {
        UniTask DownloadDependenciesAsync(object key, bool autoReleaseHandle = false, CancellationToken cToken = default);
    }
}