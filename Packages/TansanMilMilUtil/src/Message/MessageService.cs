using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    internal class MessageService : IMessageService
    {
        private readonly IMessageFrameFactory frameFactory;
        private readonly List<IMessageFrame> activeFrames = new List<IMessageFrame>();
        private readonly object lockObject = new object();

        public MessageService(IMessageFrameFactory frameFactory)
        {
            this.frameFactory = frameFactory;
        }

        public IReadOnlyCollection<IMessageFrame> GetActiveFrames()
        {
            lock (lockObject)
            {
                return new ReadOnlyCollection<IMessageFrame>(new List<IMessageFrame>(activeFrames));
            }
        }

        public async UniTask<MessageResult> ShowMessageAsync(MessageText message, MessageConfig config, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            IMessageFrame frame = frameFactory.CreateMessageFrame();
            if (frame == null)
            {
                throw new InvalidOperationException("Failed to create message frame. Ensure the factory is properly initialized.");
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

            lock (lockObject)
            {
                activeFrames.Add(frame);
            }

            await frame.OpenFrameAsync(cToken);
            await frame.RenderTextAsync(cToken);
            await frame.OpenChoicesFrameAsync(cToken);
            await frame.RenderChoicesTextsAsync(cToken);
        }

        private async UniTask HideFrameAsync(IMessageFrame frame, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            await frame.CloseFrameAsync(cToken);

            lock (lockObject)
            {
                activeFrames.Remove(frame);
            }

            frame.DestroyMessageObj();
        }
    }
}
