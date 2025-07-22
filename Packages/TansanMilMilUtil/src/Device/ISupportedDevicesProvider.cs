using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// サポートデバイス情報を提供するインターフェース
    /// 各プロジェクトで具体的なサポートデバイス情報を注入するために使用
    /// </summary>
    public interface ISupportedDevicesProvider
    {
        /// <summary>
        /// サポート対象デバイスのリストを取得
        /// </summary>
        /// <returns>サポートされるデバイスタイプのリスト</returns>
        List<DeviceType> GetSupportedDevices();

        /// <summary>
        /// 指定されたデバイスタイプがサポートされているかチェック
        /// </summary>
        /// <param name="deviceType">チェック対象のデバイスタイプ</param>
        /// <returns>サポートされている場合true</returns>
        bool IsSupported(DeviceType deviceType);
    }
}
