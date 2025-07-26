using System.Threading;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Audio;

namespace TansanMilMil.Util
{
    [RequireComponent(typeof(AudioSource))]
    [DefaultExecutionOrder(-10)]
    public class SoundManager : MonoBehaviour, IIgnoreVacuumComponent, ISoundManager
    {
        [SerializeField]
        private AudioMixer soundAudioMixer;
        private AudioSource audioSource;
        private const string MixerSE = "MasterVolume";
        private const float MaxVolume = 0;
        private const float MinVolume = -40;
        private const float AudioMixerMinVolume = -80;
        private IPlayerConfigManager playerConfigManager => PlayerConfigManager.GetInstance();

        private void Start()
        {
            SetMixerVolume(playerConfigManager.GetConfig().soundVolume);
        }

        public void Stop()
        {
            audioSource.Stop();
            audioSource.time = 0;
        }

        public float GetMixerVolume()
        {
            soundAudioMixer.GetFloat(MixerSE, out float volume);
            return volume;
        }

        public void SetMixerVolume(float volume)
        {
            soundAudioMixer.SetFloat(MixerSE, Mathf.Clamp(volume, AudioMixerMinVolume, MaxVolume));
        }

        public void Play(AudioClip se)
        {
            audioSource.time = 0;
            audioSource.volume = 1;
            audioSource.PlayOneShot(se);
        }

        public async UniTask PlayAsync(AudioClip se, CancellationToken cToken = default)
        {
            cToken.ThrowIfCancellationRequested();

            audioSource.time = 0;
            audioSource.volume = 1;
            audioSource.PlayOneShot(se);
            await UniTask.WaitUntil(() => !audioSource.isPlaying, cancellationToken: cToken);
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
