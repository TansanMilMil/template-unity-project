using System;
using R3;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public class GamePause : SingletonMonoBehaviour<GamePause>
    {
        private BehaviorSubject<bool> _onPaused = new BehaviorSubject<bool>(false);
        public Observable<bool> OnPaused => _onPaused;
        private IInputKeys inputKeys => InputKeys.GetInstance();
        private IPauseEventsRegistry pauseEventsRegistry => PauseEventsRegistry.GetInstance();

        void Start()
        {
            SetInitSubscriber();
        }

        private void SetInitSubscriber()
        {
            IDisposable waitKey = Observable.EveryUpdate()
                .Where(_ => inputKeys.AnyInputGetKeyDown(KeyRole.Pause))
                .Subscribe(_ =>
                {
                    _onPaused.OnNext(!_onPaused.Value);
                })
                .AddTo(this.GetCancellationTokenOnDestroy());

            _onPaused
                .Skip(1)
                .Subscribe(paused =>
                {
                    if (paused)
                    {
                        pauseEventsRegistry.FireOnPauseEvents();
                    }
                    else
                    {
                        pauseEventsRegistry.FireOnResumeEvents();
                    }
                })
                .AddTo(this.GetCancellationTokenOnDestroy());
        }
    }
}
