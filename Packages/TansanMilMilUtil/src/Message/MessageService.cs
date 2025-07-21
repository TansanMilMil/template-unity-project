using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public class MessageService : IMessageService
    {
        private readonly IMessageFrameFactory frameFactory;
        private readonly List<IMessageFrame> activeFrames = new List<IMessageFrame>();

        public MessageService(IMessageFrameFactory frameFactory)
        {
            this.frameFactory = frameFactory;
        }

        public IReadOnlyCollection<IMessageFrame> GetActiveFrames()
        {
            return new ReadOnlyCollection<IMessageFrame>(activeFrames);
        }

        public async UniTask<MessageResult> ShowMessageAsync(MessageText message, MessageConfig config, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            IMessageFrame frame = frameFactory.CreateMessageFrame();
            if (frame == null)
            {
                return new MessageResult(null, -1);
            }

            frame.SetMessageState(message, config);
            await ShowFrameAsync(frame, cToken);

            if (config.noClose)
            {
                return new MessageResult(frame as MessageFrameBase, -1);
            }

            MessageResult result = await frame.WaitForCloseAsync(cToken);
            await HideFrameAsync(frame, cToken);

            return result;
        }

        private async UniTask ShowFrameAsync(IMessageFrame frame, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            activeFrames.Add(frame);
            await frame.OpenFrameAsync(cToken);
            await frame.RenderTextAsync(cToken);
            await frame.OpenChoicesFrameAsync(cToken);
            await frame.RenderChoicesTextsAsync(cToken);
        }

        private async UniTask HideFrameAsync(IMessageFrame frame, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            await frame.CloseFrameAsync(cToken);
            activeFrames.Remove(frame);
            frame.DestroyMessageObj();
        }
    }
}