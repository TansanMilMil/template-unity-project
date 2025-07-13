using System;
using R3;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public class GamePause : MonoBehaviour
    {
        private static GameObject Instance;
        public static GamePause InstanceComponent;
        public BehaviorSubject<bool> onPaused = new BehaviorSubject<bool>(false);
        [SerializeField] private AudioMixer bgmAudioMixer;
        private float bgmAudioMixerVolume = 0;
        [SerializeField] private AudioMixer soundAudioMixer;
        private float soundAudioMixerVolume = 0;
        private BgmManager bgmManager;
        private SoundManager soundManager;

        private GamePause() { }

        public static GamePause GetInstance()
        {
            if (Instance == null)
            {
                throw new Exception("GamePause.Instance is null!");
            }
            if (InstanceComponent == null)
            {
                throw new Exception("GamePause.InstanceComponent is null!");
            }
            return InstanceComponent;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                // すでにロードされていたら自分自身を破棄して終了
                Destroy(gameObject);
                return;
            }
            else
            {
                // ロードされていなかったら、フラグをロード済みに設定する
                Instance = gameObject;
                InstanceComponent = gameObject.GetComponent<GamePause>();
                // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

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
                    onPaused.OnNext(!onPaused.Value);
                })
                .AddTo(this.GetCancellationTokenOnDestroy());

            onPaused
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