using UnityEngine;

namespace TansanMilMil.Util
{
    /// <summary>
    /// ジェネリック型のコンポーネントはインスペクタにアタッチできないため、ジェネリック未使用のclassを別途作成してアタッチできるようにしている
    /// </summary>
    [DefaultExecutionOrder(-30)]
    public class ConfigSaveDataManagerStringKeyStringValue : ConfigSaveDataManager<string, string>
    {
    }
}
