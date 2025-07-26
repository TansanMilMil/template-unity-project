using System;

namespace TansanMilMil.Util
{
    /// <summary>
    /// クレジットプロバイダーを管理するレジストリ
    /// </summary>
    public class CreditProviderRegistry : Singleton<CreditProviderRegistry>, ICreditProviderRegistry
    {
        private ICreditProvider _provider;

        /// <summary>
        /// クレジットプロバイダーを登録
        /// </summary>
        /// <param name="provider">クレジットプロバイダー</param>
        public void RegisterProvider(ICreditProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 登録されたクレジットプロバイダーを取得
        /// </summary>
        /// <returns>クレジットプロバイダー</returns>
        public ICreditProvider GetProvider()
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
