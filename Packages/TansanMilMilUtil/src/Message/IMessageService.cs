using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public interface IMessageService
    {
        UniTask<MessageResult> ShowMessageAsync(MessageText message, MessageConfig config, CancellationToken cToken);
        IReadOnlyCollection<IMessageFrame> GetActiveFrames();
    }
}
