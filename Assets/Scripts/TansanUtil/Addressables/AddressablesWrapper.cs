using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public class AddressablesWrapper<T> : IAddressablesWrapper<T>
    {
        private static AddressablesWrapper<T> Instance = new AddressablesWrapper<T>();

        private AddressablesWrapper() { }

        public static AddressablesWrapper<T> GetInstance()
        {
            return Instance;
        }

        public AsyncOperationHandle<T> LoadAssetAsync(string pathName)
        {
            return Addressables.LoadAssetAsync<T>(pathName);
        }

        public void Release(AsyncOperationHandle opHandle)
        {
            Addressables.Release(opHandle);
        }

        public async UniTask<T> AwaitHandle(AsyncOperationHandle<T> handle)
        {
            return await handle;
        }
    }

    public class AddressablesWrapper : IAddressablesWrapper
    {
        private static AddressablesWrapper Instance = new AddressablesWrapper();

        private AddressablesWrapper() { }

        public static AddressablesWrapper GetInstance()
        {
            return Instance;
        }

        public async UniTask DownloadDependenciesAsync(object key, bool autoReleaseHandle = false)
        {
            await Addressables.DownloadDependenciesAsync(key, autoReleaseHandle);
        }
    }
}