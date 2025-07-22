using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class SupportedDevices : Singleton<SupportedDevices>
    {
        /// <summary>
        /// サポートデバイス情報を取得
        /// プロバイダーが登録されていればそれを使用し、なければデフォルトを使用
        /// </summary>
        /// <returns>サポートされるデバイスタイプのリスト</returns>
        public List<DeviceType> GetSupportedDevices()
        {
            ISupportedDevicesProvider provider = GetProvider();
            return provider.GetSupportedDevices();
        }

        /// <summary>
        /// 指定されたデバイスタイプがサポートされているかチェック
        /// </summary>
        /// <param name="deviceType">チェック対象のデバイスタイプ</param>
        /// <returns>サポートされている場合true</returns>
        public bool IsSupported(DeviceType deviceType)
        {
            ISupportedDevicesProvider provider = GetProvider();
            return provider.IsSupported(deviceType);
        }

        /// <summary>
        /// 適切なプロバイダーを取得
        /// プロバイダーが登録されていればそれを使用し、なければデフォルトを使用
        /// </summary>
        /// <returns>サポートデバイスプロバイダー</returns>
        private ISupportedDevicesProvider GetProvider()
        {
            if (!SupportedDevicesProviderRegistry.GetInstance().IsProviderRegistered())
            {
                throw new System.InvalidOperationException("No supported devices provider is registered. Please register a provider before calling this method.");
            }

            ISupportedDevicesProvider provider = SupportedDevicesProviderRegistry.GetInstance().GetProvider();
            return provider;
        }
    }
}
