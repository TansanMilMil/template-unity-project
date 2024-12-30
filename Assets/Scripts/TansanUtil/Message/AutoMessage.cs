using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

using TMPro;
using R3;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class AutoMessage : MonoBehaviour
    {
        [SerializeField] private string messageKey;
        [SerializeField] private TableReferenceType tableReference;
        [SerializeField] private int messageWriteTextInterval = 15;
        private const int DefaultTextSoundInterval = 200;
        private int textSoundInterval = DefaultTextSoundInterval;
        [SerializeField] private AudioClip messageSe;
        [SerializeField] private TextMeshProUGUI textMesh;
        public Subject<bool> isWriting = new Subject<bool>();
        [SerializeField] private AudioSource audioSource;
        private IDisposable writeText = null;
        private IDisposable textSound = null;

        void Start()
        {
            MessagesAsync(messageKey).Forget();
        }

        private async UniTask MessagesAsync(string messageKey)
        {
            string localeTexts = await GameLocale.GetEntryValueReplacedAsync(new LocaleString(messageKey, tableReference));
            await WriteMessageAsync(localeTexts);
        }

        private async UniTask WriteMessageAsync(string text)
        {
            int writeTextInterval = messageWriteTextInterval;

            int textInterval = writeTextInterval;

            int charIndex = 0;
            bool completed = false;
            isWriting.OnNext(true);

            // textを1文字ずつ表示する
            writeText = Observable.Interval(TimeSpan.FromMilliseconds(textInterval))
                .TakeWhile(_ => charIndex < text.Length)
                .Subscribe(
                    _ =>
                    {
                        textMesh.text = $@"{textMesh.text + text[charIndex]}";
                        charIndex++;
                    },
                    onCompleted =>
                    {
                        isWriting.OnNext(false);
                        completed = true;
                    }
                )
                .AddTo(this.GetCancellationTokenOnDestroy());

            // 文字送りSEを再生する
            textSound = null;
            if (messageSe != null)
            {
                audioSource.PlayOneShot(messageSe);
                textSound = Observable.Interval(TimeSpan.FromMilliseconds(textSoundInterval))
                    .TakeWhile(_ => charIndex < text.Length)
                    .Subscribe(_ =>
                    {
                        audioSource.PlayOneShot(messageSe);
                    })
                    .AddTo(this.GetCancellationTokenOnDestroy());
            }

            await UniTask.WaitUntil(() => completed);

        }

        public void Dispose()
        {
            // 後始末
            if (writeText != null) writeText.Dispose();
            if (textSound != null) textSound.Dispose();
        }
    }
}