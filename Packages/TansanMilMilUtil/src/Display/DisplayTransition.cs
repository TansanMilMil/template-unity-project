using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class DisplayTransition : MonoBehaviour, IIgnoreVacuumComponent
    {
        public Image transition;
        private Tweener tween;
        public ArrowTransition arrowTransition;
        private void Start()
        {
            SetInitSubscriber();
        }

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

        private void SetInitSubscriber()
        {
            arrowTransition.ShowAllBlackImage
                .Subscribe(isShow =>
                {
                    transition.transform.localScale = isShow ? Vector3.one : Vector3.zero;
                })
                .AddTo(this.GetCancellationTokenOnDestroy());
            ;
        }

        public async UniTask FadeInAsync(float duration = 0.7f, Color? color = null, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            Color changeColor = color ?? Color.black;
            KillPrevAnimation();

            transition.color = Color.clear;
            transition.transform.localScale = Vector3.one;
            tween = transition.DOColor(changeColor, duration);
            await tween.WithCancellation(cToken);
        }

        public void FadeInImmediately(Color? color = null)
        {
            KillPrevAnimation();

            transition.color = color == null ? Color.black : (Color)color;
            transition.transform.localScale = Vector3.one;
        }

        public async UniTask FadeOutAsync(float duration = 0.7f, Color? color = null, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            Color changeColor = color ?? Color.black;
            KillPrevAnimation();

            transition.color = changeColor;
            transition.transform.localScale = Vector3.one;
            tween = transition.DOColor(Color.clear, duration);
            await tween.WithCancellation(cToken);

            cToken.ThrowIfCancellationRequested();
            transition.transform.localScale = Vector3.zero;
        }

        public void FadeOutImmediately()
        {
            KillPrevAnimation();

            transition.color = Color.clear;
            transition.transform.localScale = Vector3.zero;
        }

        public async UniTask ArrowFadeOutAsync(float duration = 0.7f, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            await arrowTransition.FadeOutAsync(duration, cToken);
        }

        public async UniTask ArrowFadeInAsync(float duration = 0.7f, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            await arrowTransition.FadeInAsync(duration, cToken);
        }

        private void KillPrevAnimation()
        {
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }
        }
    }
}
