using System.Threading;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Audio;

namespace TansanMilMil.Util
{
    [RequireComponent(typeof(AudioSource))]
    [DefaultExecutionOrder(-10)]
    public class SoundManager : MonoBehaviour, IIgnoreVacuumComponent
    {
        [SerializeField] private AudioMixer soundAudioMixer;
        private AudioSource audioSource;
        public const string MixerSE = "MasterVolume";
        public const float MaxVolume = 0;
        public const float MinVolume = -40;
        private const float AudioMixerMinVolume = -80;

        private void Start()
        {
            SetMixerVolume(PlayerConfigManager.GetInstance().GetConfig().soundVolume);
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
