using UnityEngine;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
namespace TansanMilMil.Util
{
    public class CommentForDeveloper : MonoBehaviour
    {
        [TextArea(3, 100)]
        public string Notes = "";
    }
}
#endif
