using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// サポートデバイスプロバイダーを管理するレジストリ
    /// </summary>
    [RequireInitializeSingleton]
    public class SupportedDevicesProviderRegistry : Singleton<SupportedDevicesProviderRegistry>, ISupportedDevicesProviderRegistry
    {
        private ISupportedDevicesProvider _provider;

        /// <summary>
        /// サポートデバイスプロバイダーを登録
        /// </summary>
        /// <param name="provider">サポートデバイスプロバイダー</param>
        public void Initialize(ISupportedDevicesProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 登録されたサポートデバイスプロバイダーを取得
        /// </summary>
        /// <returns>サポートデバイスプロバイダー</returns>
        public ISupportedDevicesProvider GetProvider()
        {
            if (_provider == null)
            {
                Debug.LogError("SupportedDevicesProvider is not initialized. Please call Initialize() before using GetProvider().");
                return null;
            }
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
