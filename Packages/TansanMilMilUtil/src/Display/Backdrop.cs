using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    public class Backdrop : MonoBehaviour
    {
        public Image transition;

        private void Update()
        {
#if UNITY_EDITOR
            // エディタ上で常時表示されると見づらいので、エディタ上では透明にする
            if (!Application.isPlaying)
            {
                MakeTransparent(transition);
            }
#endif
        }

        private void MakeTransparent(Image transition)
        {
            if (transition == null)
                return;

            Color c = transition.color;
            c.a = 0;
            transition.color = c;
        }

        public async UniTask FadeAsync(float alpha, float duration = 0.7f)
        {
            transition.transform.localScale = Vector3.one;
            await transition.DOColor(new Color(0, 0, 0, alpha), duration);
            if (alpha == 0)
                transition.transform.localScale = Vector3.zero;
        }

        public void FadeOutImmediately()
        {
            transition.color = Color.black;
            transition.transform.localScale = Vector3.zero;
        }
    }
}
