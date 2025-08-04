namespace TansanMilMil.Util
{
    public interface IRequireInitialize<T>
    {
        void Initialize(T instance);

        void AssertInitialized();
    }
}
