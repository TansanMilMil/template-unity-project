using System;
using Cysharp.Threading.Tasks;
using R3;

namespace TansanMilMil.Util
{
    public interface IGameTimer
    {
        Observable<GameTimer> OnTimeUp { get; }
        Observable<bool> OnPause { get; }
        Observable<float> OnStart { get; }
        Observable<float> TimerProgress { get; }
        void Create(float time, bool isPause = false, Func<GameTimer, UniTask> onTimeUpAsync = null);
        void Pause();
        void Resume();
        void DestroyTimer();
        float GetRemainSec();
        float GetInitRemainSec();
    }
}