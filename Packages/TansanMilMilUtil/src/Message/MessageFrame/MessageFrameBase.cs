using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public abstract class MessageFrameBase : MonoBehaviour, IMessageFrame
    {
        protected MessageText message;
        protected MessageConfig config;
        [Header("メッセージ送り時の効果音")]
        [SerializeField]
        private AudioClip textSound;

        public void SetMessageState(MessageText message, MessageConfig config)
        {
            this.message = message;
            this.config = SetDefaultConfig(config);
        }

        private MessageConfig SetDefaultConfig(MessageConfig config)
        {
            config.textSound ??= textSound;

            return config;
        }

        public abstract UniTask OpenFrameAsync(CancellationToken cToken = default);

        public abstract UniTask RenderTextAsync(CancellationToken cToken = default);

        public abstract UniTask OpenChoicesFrameAsync(CancellationToken cToken = default);

        public abstract UniTask RenderChoicesTextsAsync(CancellationToken cToken = default);

        public abstract UniTask<MessageResult> WaitForCloseAsync(CancellationToken cToken = default);

        public abstract UniTask CloseFrameAsync(CancellationToken cToken = default);

        public void DestroyMessageObj()
        {
            Destroy(gameObject);
        }
    }
}
