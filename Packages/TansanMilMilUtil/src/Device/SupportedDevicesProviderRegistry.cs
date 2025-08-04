using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// サポートデバイスプロバイダーを管理するレジストリ
    /// </summary>
    public class SupportedDevicesProviderRegistry : Singleton<SupportedDevicesProviderRegistry>, ISupportedDevicesProviderRegistry, IRequireInitialize<ISupportedDevicesProvider>
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

        public void AssertInitialized()
        {
            if (_provider == null)
            {
                throw new InvalidOperationException("SupportedDevicesProvider is not initialized. Please call Initialize() before using this method.");
            }
        }

        /// <summary>
        /// 登録されたサポートデバイスプロバイダーを取得
        /// </summary>
        /// <returns>サポートデバイスプロバイダー</returns>
        public ISupportedDevicesProvider GetProvider()
        {
            AssertInitialized();

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
