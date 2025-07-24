using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class DisplayFlash : SingletonMonoBehaviour<DisplayFlash>, IIgnoreVacuumComponent
    {
        [SerializeField]
        private GameObject flashObj;
        [SerializeField]
        private Image flash;
        [SerializeField]
        private Color defaultColor = new Color(1, 1, 1, 1);

        public async UniTask FlashAsync(float duration = 0.8f, Color? flashColor = null, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            flash.color = flashColor == null ? defaultColor : (Color)flashColor;
            Color targetColor = new Color(flash.color.r, flash.color.g, flash.color.b, 0);
            flashObj.SetActive(true);
            await flash.DOColor(targetColor, duration).WithCancellation(cToken);
            flashObj.SetActive(false);
        }

        public async UniTask WhiteOutAsync(float duration = 1.5f, Color? flashColor = null, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            Color targetColor = flashColor == null ? defaultColor : (Color)flashColor;
            flash.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0);
            flashObj.SetActive(true);
            await flash.DOColor(targetColor, duration).WithCancellation(cToken);
        }

        public async UniTask RemoveWhiteOutAsync(float duration = 1.5f, Color? flashColor = null, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            Color targetColor = flashColor == null ? defaultColor : (Color)flashColor;
            flash.color = new Color(targetColor.r, targetColor.g, targetColor.b, 1);
            flashObj.SetActive(true);
            await flash.DOColor(new Color(targetColor.r, targetColor.g, targetColor.b, 0), duration).WithCancellation(cToken);
            flashObj.SetActive(false);
        }

        public void RemoveWhiteOutImmediately()
        {
            flashObj.SetActive(false);
        }
    }
}
