using UnityEngine;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
namespace TansanMilMil.Util
{
    /// <summary>
    /// 開発者向けのコメントを表示したい時にアタッチしてください
    /// </summary>
    public class CommentForDeveloper : MonoBehaviour
    {
        [TextArea(3, 100)]
        public string Notes = "";
    }
}
#endif
