using UnityEngine.ResourceManagement.AsyncOperations;

namespace TansanMilMil.Util
{
    public class AddressablesKeeperItem<T>
    {
        public string pathName { get; private set; }
        public T asset { get; private set; }
        public AsyncOperationHandle<T>? handle { get; private set; }

        public AddressablesKeeperItem(string pathName, T asset, AsyncOperationHandle<T>? handle)
        {
            this.pathName = pathName;
            this.asset = asset;
            this.handle = handle;
        }
    }
}