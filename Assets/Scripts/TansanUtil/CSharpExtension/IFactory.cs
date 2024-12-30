namespace TansanMilMil.Util
{
    public interface IFactory<T, S>
    {
        public T Create(S id);
    }
}