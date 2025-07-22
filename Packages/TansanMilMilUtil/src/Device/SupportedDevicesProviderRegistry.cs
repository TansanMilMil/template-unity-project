namespace TansanMilMil.Util
{
    /// <summary>
    /// サポートデバイスプロバイダーを管理するレジストリ
    /// </summary>
    public class SupportedDevicesProviderRegistry : Singleton<SupportedDevicesProviderRegistry>
    {
        private ISupportedDevicesProvider _provider;

        /// <summary>
        /// サポートデバイスプロバイダーを登録
        /// </summary>
        /// <param name="provider">サポートデバイスプロバイダー</param>
        public void RegisterProvider(ISupportedDevicesProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 登録されたサポートデバイスプロバイダーを取得
        /// </summary>
        /// <returns>サポートデバイスプロバイダー</returns>
        public ISupportedDevicesProvider GetProvider()
        {
            return _provider;
        }

        /// <summary>
        /// プロバイダーが登録されているかチェック
        /// </summary>
        /// <returns>プロバイダーが登録されている場合true</returns>
        public bool IsProviderRegistered()
        {
            return _provider != null;
        }
    }
}