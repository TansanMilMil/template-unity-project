using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public abstract class MessageFrameBase : MonoBehaviour
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

        public abstract UniTask OpenFrameAsync(CancellationToken cToken);

        public abstract UniTask RenderTextAsync(CancellationToken cToken);

        public abstract UniTask OpenChoicesFrameAsync(CancellationToken cToken);

        public abstract UniTask RenderChoicesTextsAsync(CancellationToken cToken);

        public abstract UniTask<MessageResult> WaitForCloseAsync(CancellationToken cToken);

        public abstract UniTask CloseFrameAsync(CancellationToken cToken);

        public void DestroyMessageObj()
        {
            Destroy(gameObject);
        }
    }
}
