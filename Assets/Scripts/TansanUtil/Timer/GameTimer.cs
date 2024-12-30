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
        public Subject<GameTimer> onTimeUp = new Subject<GameTimer>();
        public BehaviorSubject<bool> onPause = new BehaviorSubject<bool>(false);
        public Subject<float> onStart = new Subject<float>();
        public Subject<float> timerProgress = new Subject<float>();

        private void Awake()
        {
            // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (remainSec > 0 && !onPause.Value)
            {
                remainSec -= Time.deltaTime;
                if (remainSec <= 0)
                {
                    onTimeUp.OnNext(this);
                }
                else
                {
                    timerProgress.OnNext(remainSec);
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
                onPause.OnNext(true);
            }

            onStart.OnNext(remainSec);

            if (this.onTimeUpAsync != null)
            {
                onTimeUp
                    .Subscribe(async timer => await this.onTimeUpAsync(timer))
                    .AddTo(this.GetCancellationTokenOnDestroy());
            }
        }

        public void Pause()
        {
            onPause.OnNext(true);
        }

        public void UnPause()
        {
            onPause.OnNext(false);
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