using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// ゲーム全体のロケール設定を初期化するコンポーネント
    /// ジェネリック型のコンポーネントはインスペクタにアタッチできないため、ジェネリック未使用のclassを別途作成してアタッチできるようにしている
    /// </summary>
    [DefaultExecutionOrder(-10)]
    public class GameLocaleInitializerStringKeyStringValue : GameLocaleInitializer<string, string>
    {
    }
}
