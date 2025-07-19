using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class MessageManager : MonoBehaviour
    {
        [Header("textの置き換えロジックを実装したコンポーネントをアタッチすると置き換えが行われます")]
        [SerializeField]
        private List<TextReplaceStrategy> textReplaceStrategies = new List<TextReplaceStrategy>();
        [Header("メッセージ送り時の効果音")]
        [SerializeField]
        private AudioClip textSound;
        private GameObject messageFramePrefab;
        private readonly List<MessageFrameBase> activeFrames = new List<MessageFrameBase>();

        public async UniTask<MessageResult> MessageAsync(MessageText message, MessageConfig config, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            config = SetDefaultConfig(config);
            message = ReplaceTexts(message);

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

        public async UniTask HideMessageAsync(MessageFrameBase frame, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            await frame.CloseFrameAsync(cToken);
            activeFrames.Remove(frame);
            frame.DestroyMessageObj();
        }

        private MessageConfig SetDefaultConfig(MessageConfig config)
        {
            config.textSound ??= textSound;

            return config;
        }

        private MessageText ReplaceTexts(MessageText message)
        {
            if (textReplaceStrategies.Count == 0)
                return message;

            var replacer = new MessageTextReplacer(textReplaceStrategies);
            message.text = replacer.Replace(message.text);

            for (int i = 0; i < message.choices.Count; i++)
            {
                message.choices[i] = replacer.Replace(message.choices[i]);
            }

            return message;
        }
    }
}
