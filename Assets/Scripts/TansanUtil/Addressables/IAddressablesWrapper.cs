using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public interface IAddressablesWrapper<T>
    {
        public AsyncOperationHandle<T> LoadAssetAsync(string pathName);

        public void Release(AsyncOperationHandle opHandle);

        public UniTask<T> AwaitHandle(AsyncOperationHandle<T> handle);
    }

    public interface IAddressablesWrapper
    {
        public UniTask DownloadDependenciesAsync(object key, bool autoReleaseHandle = false);
    }
}