namespace TansanMilMil.Util
{
    public class KVStoreValueConverter<TValue> : Singleton<KVStoreValueConverter<TValue>>, IStoreValueConverter<TValue>
    {
        public TValue Convert(string value)
        {
            if (typeof(TValue) == typeof(string))
            {
                return (TValue)(object)value;
            }

            throw new System.NotImplementedException("Conversion from string to TValue is not implemented.");
        }

        public string Convert(TValue value)
        {
            // TValueがstringの時はそのまま返す
            if (typeof(TValue) == typeof(string))
            {
                return value.ToString();
            }

            throw new System.NotImplementedException("Conversion from TValue to string is not implemented.");
        }
    }
}
