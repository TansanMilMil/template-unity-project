using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace TansanMilMil.Util
{
    public class SkipEvent : MonoBehaviour
    {
        /// <summary>
        /// スキップをするまでの時間の閾値。
        /// </summary>
        [SerializeField]
        private float threshold = 0.6f;
        private float pushSkipKeyTime = 0.0f;
        private bool skipKeyDown = false;
        private Subject<bool> _onSkipEvent = new Subject<bool>();
        public Observable<bool> OnSkipEvent => _onSkipEvent;
        private BehaviorSubject<bool> _observing = new BehaviorSubject<bool>(false);
        public Observable<bool> Observing => _observing;
        private IInputKeys inputKeys => InputKeys.GetInstance();

        /// <summary>
        /// スキップイベントの観測を開始し、引数で指定された非同期アクションにサブスクライブします。
        /// </summary>
        /// <param name="actionAsync">非同期アクション</param>
        public void StartObserveAndSubscribe(Func<UniTask> actionAsync)
        {
            StartObserve();
            SubscribeSkipEvent(actionAsync);
        }

        private Observable<bool> StartObserve()
        {
            _observing.OnNext(true);
            return OnSkipEvent;
        }

        private void SubscribeSkipEvent(Func<UniTask> actionAsync)
        {
            _onSkipEvent
                .Where(_ => _observing.Value)
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
            _observing.OnNext(false);
        }

        private void Update()
        {
            if (!_observing.Value)
                return;

            if (inputKeys.AnyInputGetKey(KeyRole.Skip) || skipKeyDown)
            {
                pushSkipKeyTime += Time.deltaTime;
                if (pushSkipKeyTime > threshold)
                {
                    _onSkipEvent.OnNext(true);
                    pushSkipKeyTime = 0.0f;
                    StopObserve();
                }
            }
            else
            {
                pushSkipKeyTime = 0.0f;
            }
        }

        public void SkipKeyDown()
        {
            skipKeyDown = true;
        }

        public void SkipKeyUp()
        {
            skipKeyDown = false;
        }

        public float GetPushSkipKeyTimeRatio()
        {
            return Mathf.Clamp(pushSkipKeyTime / threshold, 0, 1);
        }
    }
}
