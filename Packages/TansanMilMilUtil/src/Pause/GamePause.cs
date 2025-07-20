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
        [SerializeField] private AudioMixer bgmAudioMixer;
        private float bgmAudioMixerVolume = 0;
        [SerializeField] private AudioMixer soundAudioMixer;
        private float soundAudioMixerVolume = 0;
        private BgmManager bgmManager;
        private SoundManager soundManager;

        void Start()
        {
            bgmManager = GameObjectHolder.GetInstance().FindComponentBy<BgmManager>("BgmManager");
            soundManager = GameObjectHolder.GetInstance().FindComponentBy<SoundManager>("SoundManager");

            SetInitSubscriber();
        }

        private void SetInitSubscriber()
        {
            IDisposable waitKey = Observable.EveryUpdate()
                .Where(_ => InputKeys.GetInstance().AnyInputGetKeyDown(KeyRole.Pause))
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
                        TimeScaleManager.SetTimeScale(0);

                        bgmAudioMixerVolume = bgmManager.GetMixerVolume();
                        bgmManager.SetMixerVolume(bgmAudioMixerVolume - 20);

                        soundAudioMixerVolume = soundManager.GetMixerVolume();
                        soundManager.SetMixerVolume(soundAudioMixerVolume - 20);
                    }
                    else
                    {
                        TimeScaleManager.ResetTimeScale();

                        bgmManager.SetMixerVolume(bgmAudioMixerVolume);
                        soundManager.SetMixerVolume(soundAudioMixerVolume);
                    }
                })
                .AddTo(this.GetCancellationTokenOnDestroy());
        }
    }
}
