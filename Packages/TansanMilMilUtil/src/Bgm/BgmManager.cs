using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using R3;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Audio;

namespace TansanMilMil.Util
{
    /// <summary>
    /// Sceneを跨いで動作するBGM管理クラス。Singleton。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [DefaultExecutionOrder(-20)]
    public class BgmManager : SingletonMonoBehaviour<BgmManager>, IIgnoreVacuumComponent
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

        public float timeBeforeMovingScene = 0;
        private AssetsKeeper<AudioClip> audioKeeper;
        private IBgmFactory bgmFactory = BgmFactory.GetInstance();
        private IPlayerConfigManager playerConfigManager => PlayerConfigManager.GetInstance();
        private IAssetsTypeSettingRegistry assetsTypeSettingRegistry => AssetsTypeSettingRegistry.GetInstance();

        protected override void OnSingletonStart()
        {
            SetMixerVolume(playerConfigManager.GetConfig().bgmVolume);

            audioKeeper = new AssetsKeeperFactory(assetsTypeSettingRegistry.GetAssetsTypeSetting()).Create<AudioClip>(autoRelease: true);
        }

        protected override void OnSingletonUpdate()
        {
            if (audioSource != null && audioSource.clip != null)
            {
                CheckLoop();
            }
        }

        public void ResetStaticParams()
        {
            timeBeforeMovingScene = 0;
        }

        public async UniTask<AudioClip> LoadAssetAsync(string bgmPath, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            AudioClip audioClip = await audioKeeper.LoadAssetAsync(bgmPath, cToken);
            return audioClip;
        }

        public void ReleaseAsset(string bgmPath)
        {
            audioKeeper.ReleaseAsset(bgmPath);
        }

        public void ReleaseAllAssets()
        {
            audioKeeper.ReleaseAllAssets();
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
                currentMusic = bgmFactory.Create(audioClip.name);
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

        public async UniTask PlayByPathAsync(string bgmPath, float startTime = Bgm.NoTimeSetting, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            if (bgmPath == bgmFactory.Create(bgmPath).filePath)
            {
                Stop();
                return;
            }

            AudioClip audio = await LoadAssetAsync(bgmPath, cToken);

            cToken.ThrowIfCancellationRequested();
            Play(audio, startTime);
        }

        public async UniTask<bool> PlayWithFadeInAsync(AudioClip audioClip = null, float duration = 1.0f, float startTime = Bgm.NoTimeSetting, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            KillAllAsyncEffect();
            SetUp(audioClip, startTime);
            audioSource.volume = 0;
            audioSource.Play();

            fadeIn = DOTween.To(() => audioSource.volume, (x) => audioSource.volume = x, maxVolume, duration).SetEase(Ease.Linear);
            await fadeIn.WithCancellation(cToken);

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

        public async UniTask<bool> FadeOutAsync(float duration = 1.0f, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            KillAllAsyncEffect();
            fadeOut = DOTween.To(() => audioSource.volume, (x) => audioSource.volume = x, 0f, duration).SetEase(Ease.Linear);
            await fadeOut.WithCancellation(cToken);

            cToken.ThrowIfCancellationRequested();
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
