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

        public async UniTask FadeAsync(float alpha, float duration = 0.7f)
        {
            transition.transform.localScale = Vector3.one;
            await transition.DOColor(new Color(0, 0, 0, alpha), duration);
            if (alpha == 0) transition.transform.localScale = Vector3.zero;
        }

        public void FadeOutImmediately()
        {
            transition.color = Color.black;
            transition.transform.localScale = Vector3.zero;
        }
    }
}