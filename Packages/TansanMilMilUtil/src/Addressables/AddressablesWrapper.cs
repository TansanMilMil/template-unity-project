using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public class AddressablesWrapper<T> : IAddressablesWrapper<T>
    {
        private static readonly object lockObject = new object();
        private static AddressablesWrapper<T> instance;

        private AddressablesWrapper() { }

        public static AddressablesWrapper<T> GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new AddressablesWrapper<T>();
                    }
                }
            }
            return instance;
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

    public class AddressablesWrapper : IAddressablesDownloader
    {
        private static readonly object lockObject = new object();
        private static AddressablesWrapper instance;

        private AddressablesWrapper() { }

        public static AddressablesWrapper GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new AddressablesWrapper();
                    }
                }
            }
            return instance;
        }

        public async UniTask DownloadDependenciesAsync(object key, bool autoReleaseHandle = false)
        {
            await Addressables.DownloadDependenciesAsync(key, autoReleaseHandle);
        }
    }
}