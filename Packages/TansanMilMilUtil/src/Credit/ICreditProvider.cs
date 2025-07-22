using System.Collections.Generic;

namespace TansanMilMil.Util
{
    /// <summary>
    /// クレジットデータを提供するインターフェース
    /// 各プロジェクトで具体的なクレジット情報を注入するために使用
    /// </summary>
    public interface ICreditProvider
    {
        /// <summary>
        /// クレジットリストを取得
        /// </summary>
        /// <returns>クレジットのリスト</returns>
        List<Credit> GetCredits();
    }
}