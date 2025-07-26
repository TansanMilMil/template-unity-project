using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public interface ISoundManager
    {
        void Stop();
        float GetMixerVolume();
        void SetMixerVolume(float volume);
        void Play(AudioClip se);
        UniTask PlayAsync(AudioClip se, CancellationToken cToken = default);
        float ConvertSliderValueToVolume(float sliderValue);
    }
}