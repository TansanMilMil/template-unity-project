using System.Threading;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public interface IMessageFrame
    {
        void SetMessageState(MessageText message, MessageConfig config);
        UniTask OpenFrameAsync(CancellationToken cToken);
        UniTask RenderTextAsync(CancellationToken cToken);
        UniTask OpenChoicesFrameAsync(CancellationToken cToken);
        UniTask RenderChoicesTextsAsync(CancellationToken cToken);
        UniTask<MessageResult> WaitForCloseAsync(CancellationToken cToken);
        UniTask CloseFrameAsync(CancellationToken cToken);
        void DestroyMessageObj();
    }
}