using UnityEngine;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-30)]
    public class InputKeyBindsManager : MonoBehaviour
    {
        private IInputKeys inputKeys => InputKeys.GetInstance();
        private void Start()
        {
            inputKeys.InitKeyBinds();
        }
    }
}