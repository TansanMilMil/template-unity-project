using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public abstract class MessageFrameBase : MonoBehaviour
    {
        protected MessageText message;
        protected MessageConfig config;

        public void SetMessageState(MessageText message, MessageConfig config)
        {
            this.message = message;
            this.config = config;
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
