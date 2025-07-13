using UnityEngine;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-30)]
    public class InputKeyBindsManager : MonoBehaviour
    {
        private void Start()
        {
            InputKeys.GetInstance().InitKeyBinds();
        }
    }
}