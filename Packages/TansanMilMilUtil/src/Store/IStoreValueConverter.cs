namespace TansanMilMil.Util
{
    public interface IStoreValueConverter<TValue>
    {
        TValue Convert(string json);
        string Convert(TValue value);
    }
}
