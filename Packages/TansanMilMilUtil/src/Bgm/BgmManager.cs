using System;
using System.IO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using R3;
using UnityEngine;
using UnityEngine.Audio;

namespace TansanMilMil.Util
{
    /// <summary>
    /// Sceneを跨いで動作するBGM管理クラス。Singleton。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [DefaultExecutionOrder(-20)]
    public class BgmManager : MonoBehaviour, IIgnoreVacuumComponent
    {
        [SerializeField] private AudioMixer bgmAudioMixer;
        public AudioSource audioSource;
        private Bgm currentMusic;
        private float maxVolume = 1;
        private TweenerCore<float, float, FloatOptions> fadeIn;
        private TweenerCore<float, float, FloatOptions> fadeOut;
        public const string MixerBGM = "MasterVolume";
        public const float MaxVolume = 0;
        public const float MinVolume = -40;
        private const float AudioMixerMinVolume = -80;

        private static GameObject Instance;
        private static BgmManager InstanceComponent;
        public static float TimeBeforeMovingScene = 0;
        private static AssetsKeeper<AudioClip> AudioKeeper = AssetsTypeSettings.NewAssetsKeeper<AudioClip>(autoRelease: true);

        private BgmManager() { }

        public static BgmManager GetInstance()
        {
            if (Instance == null)
            {
                throw new Exception("BgmManager.Instance is null!");
            }
            if (InstanceComponent == null)
            {
                throw new Exception("BgmManager.InstanceComponent is null!");
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
                InstanceComponent = gameObject.GetComponent<BgmManager>();
                // ルート階層にないとDontDestroyOnLoadできないので強制移動させる
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            SetMixerVolume(PlayerConfigManager.GetInstance().GetConfig().bgmVolume);
        }

        private void Update()
        {
            if (audioSource != null && audioSource.clip != null)
            {
                CheckLoop();
            }
        }

        public static void ResetStaticParams()
        {
            TimeBeforeMovingScene = 0;
        }

        public async UniTask<AudioClip> LoadAssetAsync(string bgmPath)
        {
            AudioClip audioClip = await AudioKeeper.LoadAssetAsync(bgmPath);
            return audioClip;
        }

        public void ReleaseAsset(string bgmPath)
        {
            AudioKeeper.ReleaseAsset(bgmPath);
        }

        public void ReleaseAllAssets()
        {
            AudioKeeper.ReleaseAllAssets();
        }

        public float GetMixerVolume()
        {
            bgmAudioMixer.GetFloat(MixerBGM, out float volume);
            return volume;
        }

        public void SetMixerVolume(float volume)
        {
            bgmAudioMixer.SetFloat(MixerBGM, Mathf.Clamp(volume, AudioMixerMinVolume, MaxVolume));
        }

        private void CheckLoop()
        {
            if (currentMusic == null || !currentMusic.isLoop ||
                (currentMusic.loopStartTime == Bgm.NoTimeSetting && currentMusic.loopEndTime == Bgm.NoTimeSetting))
            {
                return;
            }

            if (audioSource.time >= currentMusic.loopEndTime || (!audioSource.isPlaying && audioSource.time == 0))
            {
                audioSource.time = currentMusic.loopStartTime;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }

        public void Stop()
        {
            KillAllAsyncEffect();
            currentMusic = null;
            audioSource.Stop();
            audioSource.time = 0;
        }

        private void SetUp(AudioClip audioClip, float startTime)
        {
            if (audioClip != null)
            {
                currentMusic = BgmFactory.GetInstance().Create(audioClip.name);
                audioSource.clip = audioClip;
                maxVolume = currentMusic.maxVolume;
                audioSource.volume = currentMusic.maxVolume;
            }
            else
            {
                maxVolume = 1;
                audioSource.volume = 1;
            }

            audioSource.loop = currentMusic.loopStartTime == Bgm.NoTimeSetting && currentMusic.loopEndTime == Bgm.NoTimeSetting;
            if (startTime == Bgm.NoTimeSetting)
            {
                audioSource.time = currentMusic.initStartTime;
            }
            else
            {
                audioSource.time = startTime;
            }
        }

        public void Play(AudioClip audioClip = null, float startTime = Bgm.NoTimeSetting)
        {
            if (currentMusic != null &&
                Path.GetFileNameWithoutExtension(currentMusic.filePath) == audioClip.name &&
                startTime == Bgm.NoTimeSetting)
            {
                return;
            }

            KillAllAsyncEffect();
            SetUp(audioClip, startTime);

            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }

        public async UniTask PlayByPathAsync(string bgmPath, float startTime = Bgm.NoTimeSetting)
        {
            if (bgmPath == BgmFactory.GetInstance().Create(bgmPath).filePath)
            {
                Stop();
                return;
            }
            AudioClip audio = await LoadAssetAsync(bgmPath);
            Play(audio, startTime);
        }

        public async UniTask<bool> PlayWithFadeInAsync(AudioClip audioClip = null, float duration = 1.0f, float startTime = Bgm.NoTimeSetting)
        {
            KillAllAsyncEffect();
            SetUp(audioClip, startTime);
            audioSource.volume = 0;
            audioSource.Play();

            fadeIn = DOTween.To(() => audioSource.volume, (x) => audioSource.volume = x, maxVolume, duration).SetEase(Ease.Linear);
            await fadeIn;

            return true;
        }

        private void KillFadeIn()
        {
            if (fadeIn != null && !fadeIn.IsComplete())
            {
                fadeIn.Kill();
                fadeIn = null;
            }
        }

        public async UniTask<bool> FadeOutAsync(float duration = 1.0f)
        {
            KillAllAsyncEffect();
            fadeOut = DOTween.To(() => audioSource.volume, (x) => audioSource.volume = x, 0f, duration).SetEase(Ease.Linear);
            await fadeOut;

            audioSource.volume = 0;

            return true;
        }

        private void KillFadeOut()
        {
            if (fadeOut != null && !fadeOut.IsComplete())
            {
                fadeOut.Kill();
                fadeOut = null;
            }
        }

        private void KillAllAsyncEffect()
        {
            KillFadeIn();
            KillFadeOut();
        }

        /// <summary>
        /// ミュートを実現するために<see cref="MaxVolume"/>だった場合はAudioMixerの最小値を返す
        /// (<see cref="MaxVolume"/>をそのままreturnしてしまうと微妙に音が出てしまう)
        /// </summary>
        public float ConvertSliderValueToVolume(float sliderValue)
        {
            if (Mathf.Approximately(sliderValue, MinVolume))
            {
                return AudioMixerMinVolume;
            }
            return Mathf.Clamp(sliderValue, AudioMixerMinVolume, MaxVolume);
        }
    }
}
