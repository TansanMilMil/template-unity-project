using System.Threading;
using Cysharp.Threading.Tasks;

namespace TansanMilMil.Util
{
    public interface IMessageManager
    {
        UniTask<MessageResult> MessageAsync(MessageText message, MessageConfig config, CancellationToken cToken = default);
    }
}