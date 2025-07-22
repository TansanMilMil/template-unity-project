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
    [RequireComponent(typeof(AudioSource))]
    public abstract class BgmManagerBase : MonoBehaviour, IIgnoreVacuumComponent
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

        private void Awake()
        {
            AwakeExtension();
        }

        protected abstract void AwakeExtension();

        private void Start()
        {
            SetMixerVolume(PlayerConfigManager.GetInstance().GetConfig().bgmVolume);

            StartExtensionAsync();
        }

        protected abstract UniTask StartExtensionAsync();

        private void Update()
        {
            if (audioSource != null && audioSource.clip != null)
            {
                CheckLoop();
            }
        }

        public abstract UniTask<AudioClip> LoadAssetAsync(string bgmPath);

        public abstract void ReleaseAsset(string bgmPath);

        public abstract void ReleaseAllAssets();

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
            if (currentMusic == null || !currentMusic.isLoop || (currentMusic.loopStartTime == -1 && currentMusic.loopEndTime == -1))
            {
                return;
            }

            if (audioSource.time >= currentMusic.loopEndTime || (!audioSource.isPlaying && audioSource.time == 0))
            {
                audioSource.time = currentMusic.loopStartTime;
                if (!audioSource.isPlaying)
                    audioSource.Play();
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
                currentMusic = new BgmFactory().Create(audioClip.name);
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
            audioSource.Play();
        }

        public async UniTask PlayByBgmAsync(BgmType bgm, float startTime = Bgm.NoTimeSetting)
        {
            if (bgm == BgmType.StopBgm)
            {
                Stop();
                return;
            }
            await PlayByPathAsync(BgmFactory.GetBgmPath(bgm), startTime);
        }

        public async UniTask PlayByPathAsync(string bgmPath, float startTime = Bgm.NoTimeSetting)
        {
            if (bgmPath == BgmFactory.GetBgmPath(BgmType.StopBgm))
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
