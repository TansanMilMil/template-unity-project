using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class GameTimer : MonoBehaviour
    {
        private float initRemainSec = 0;
        private float remainSec = 0;
        private Func<GameTimer, UniTask> onTimeUpAsync = null;
        private Subject<GameTimer> _onTimeUp = new Subject<GameTimer>();
        private BehaviorSubject<bool> _onPause = new BehaviorSubject<bool>(false);
        private Subject<float> _onStart = new Subject<float>();
        private Subject<float> _timerProgress = new Subject<float>();

        public Observable<GameTimer> OnTimeUp => _onTimeUp;
        public Observable<bool> OnPause => _onPause;
        public Observable<float> OnStart => _onStart;
        public Observable<float> TimerProgress => _timerProgress;

        private void Awake()
        {
            // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (remainSec > 0 && !_onPause.Value)
            {
                remainSec -= Time.deltaTime;
                if (remainSec <= 0)
                {
                    _onTimeUp.OnNext(this);
                }
                else
                {
                    _timerProgress.OnNext(remainSec);
                }
            }
        }

        public void Create(float time, bool isPause = false, Func<GameTimer, UniTask> onTimeUpAsync = null)
        {
            remainSec = time;
            initRemainSec = time;
            this.onTimeUpAsync = onTimeUpAsync;

            if (isPause)
            {
                _onPause.OnNext(true);
            }

            _onStart.OnNext(remainSec);

            if (this.onTimeUpAsync != null)
            {
                _onTimeUp
                    .Subscribe(async timer => await this.onTimeUpAsync(timer))
                    .AddTo(this.GetCancellationTokenOnDestroy());
            }
        }

        public void Pause()
        {
            _onPause.OnNext(true);
        }

        public void Resume()
        {
            _onPause.OnNext(false);
        }

        public void DestroyTimer()
        {
            Destroy(gameObject);
        }

        public float GetRemainSec()
        {
            return remainSec;
        }

        public float GetInitRemainSec()
        {
            return initRemainSec;
        }
    }
}
