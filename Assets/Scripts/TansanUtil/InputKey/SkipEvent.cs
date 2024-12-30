using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    public class SkipEvent : MonoBehaviour
    {
        private const float Threshold = 0.6f;
        private float pushSkipKeyTime = 0.0f;
        [SerializeField] private GameObject wrapper;
        [SerializeField] private Image thresholdImage;
        private bool skipKeyDown = false;
        public Subject<bool> onSkipEvent = new Subject<bool>();
        public bool observing { get; private set; } = false;

        private Subject<bool> StartObserve()
        {
            wrapper.SetActive(true);
            observing = true;
            return onSkipEvent;
        }

        public void StartObserveAndSubscribe(Func<UniTask> actionAsync)
        {
            StartObserve();
            SubscribeSkipEvent(actionAsync);
        }

        private void SubscribeSkipEvent(Func<UniTask> actionAsync)
        {
            onSkipEvent
                .Where(_ => observing)
                .Subscribe(async _ =>
                {
                    try
                    {
                        await actionAsync.Invoke();
                    }
                    catch (OperationCanceledException e)
                    {
                        Debug.Log(e);
                    }
                })
                .AddTo(this.GetCancellationTokenOnDestroy());
        }

        public void StopObserve()
        {
            pushSkipKeyTime = 0.0f;
            wrapper.SetActive(false);
            observing = false;
        }

        private void Update()
        {
            if (!observing) return;

            if (InputKeys.GetInstance().AnyInputGetKey(KeyRole.Skip) || skipKeyDown)
            {
                pushSkipKeyTime += Time.deltaTime;
                if (pushSkipKeyTime > Threshold)
                {
                    onSkipEvent.OnNext(true);
                    pushSkipKeyTime = 0.0f;
                    StopObserve();
                }
            }
            else
            {
                pushSkipKeyTime = 0.0f;
            }

            RenderThreshold();
        }

        public void OnButtonDown()
        {
            skipKeyDown = true;
        }

        public void OnButtonUp()
        {
            skipKeyDown = false;
        }

        private void RenderThreshold()
        {
            thresholdImage.fillAmount = Mathf.Clamp(pushSkipKeyTime / Threshold, 0, 1);
        }
    }
}