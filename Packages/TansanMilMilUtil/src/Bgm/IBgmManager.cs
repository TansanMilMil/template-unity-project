using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public interface IBgmManager
    {
        void ResetStaticParams();
        UniTask<AudioClip> LoadAssetAsync(string bgmPath, CancellationToken cToken = default);
        void ReleaseAsset(string bgmPath);
        void ReleaseAllAssets();
        float GetMixerVolume();
        void SetMixerVolume(float volume);
        void Stop();
        void Play(AudioClip audioClip = null, float startTime = Bgm.NoTimeSetting);
        UniTask PlayByPathAsync(string bgmPath, float startTime = Bgm.NoTimeSetting, CancellationToken cToken = default);
        UniTask<bool> PlayWithFadeInAsync(AudioClip audioClip = null, float duration = 1.0f, float startTime = Bgm.NoTimeSetting, CancellationToken cToken = default);
        UniTask<bool> FadeOutAsync(float duration = 1.0f, CancellationToken cToken = default);
        float ConvertSliderValueToVolume(float sliderValue);
    }
}
