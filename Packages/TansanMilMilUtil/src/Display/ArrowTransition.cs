using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class ArrowTransition : MonoBehaviour
    {
        public List<GameObject> leftArrows;
        public List<GameObject> rightArrows;
        private Sequence tween;
        private const float leftStartPosX = -250f;
        private const float leftEndPosX = 180;
        private const float rightStartPosX = 250f;
        private const float rightEndPosX = -180;
        private Subject<bool> _showAllBlackImage = new Subject<bool>();
        public Observable<bool> ShowAllBlackImage => _showAllBlackImage;

        public async UniTask FadeOutAsync(float duration = 0.7f)
        {
            KillPrevAnimation();
            SetPosXImmediately(leftArrowPosX: leftEndPosX, rightArrowPosX: rightEndPosX);
            _showAllBlackImage.OnNext(false);

            tween = DOTween.Sequence();
            foreach (var leftArrow in leftArrows)
            {
                RectTransform rect = leftArrow.transform as RectTransform;
                _ = tween.Join(rect.DOAnchorPosX(leftStartPosX, duration).SetEase(Ease.Linear));
            }
            foreach (var rightArrow in rightArrows)
            {
                RectTransform rect = rightArrow.transform as RectTransform;
                _ = tween.Join(rect.DOAnchorPosX(rightStartPosX, duration).SetEase(Ease.Linear));
            }
            await tween;
        }

        public async UniTask FadeInAsync(float duration = 0.7f)
        {
            KillPrevAnimation();
            SetPosXImmediately();
            _showAllBlackImage.OnNext(false);

            tween = DOTween.Sequence();
            foreach (var leftArrow in leftArrows)
            {
                RectTransform rect = leftArrow.transform as RectTransform;
                _ = tween.Join(rect.DOAnchorPosX(leftEndPosX, duration).SetEase(Ease.Linear));
            }
            foreach (var rightArrow in rightArrows)
            {
                RectTransform rect = rightArrow.transform as RectTransform;
                _ = tween.Join(rect.DOAnchorPosX(rightEndPosX, duration).SetEase(Ease.Linear));
            }
            await tween;
        }

        private void SetPosXImmediately(float leftArrowPosX = leftStartPosX, float rightArrowPosX = rightStartPosX)
        {
            foreach (var leftArrow in leftArrows)
            {
                RectTransform rect = leftArrow.transform as RectTransform;
                rect.anchoredPosition = new Vector2(leftArrowPosX, rect.anchoredPosition.y);
            }
            foreach (var rightArrow in rightArrows)
            {
                RectTransform rect = rightArrow.transform as RectTransform;
                rect.anchoredPosition = new Vector2(rightArrowPosX, rect.anchoredPosition.y);
            }
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
