using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class MessageManager : MonoBehaviour
    {
        private GameObject messageFramePrefab;
        private readonly ICollection<MessageFrameBase> activeFrames = new List<MessageFrameBase>();

        public async UniTask<MessageResult> MessageAsync(MessageText message, MessageConfig config, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            GameObject messageObj = Instantiate(messageFramePrefab, transform);
            MessageFrameBase frame = messageObj.GetComponent<MessageFrameBase>();
            if (frame == null)
            {
                Debug.LogError("MessageFrameBase component not found on the prefab.");
                return new MessageResult(frame, -1);
            }
            frame.SetMessageState(message, config);

            await ShowMessageAsync(frame, cToken);

            if (config.noClose)
            {
                return new MessageResult(frame, -1);
            }

            MessageResult result = await frame.WaitForCloseAsync(cToken);
            await HideMessageAsync(frame, cToken);

            return result;
        }

        private async UniTask ShowMessageAsync(MessageFrameBase frame, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            activeFrames.Add(frame);
            await frame.OpenFrameAsync(cToken);
            await frame.RenderTextAsync(cToken);
            await frame.OpenChoicesFrameAsync(cToken);
            await frame.RenderChoicesTextsAsync(cToken);
        }

        private async UniTask HideMessageAsync(MessageFrameBase frame, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            await frame.CloseFrameAsync(cToken);
            activeFrames.Remove(frame);
            frame.DestroyMessageObj();
        }
    }
}
