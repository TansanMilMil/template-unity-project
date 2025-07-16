using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public abstract class MessageBase : MonoBehaviour
    {
        [Header("textの置き換えロジックを実装したコンポーネントをアタッチすると置き換えが行われます")]
        [SerializeField]
        private List<TextReplaceStrategy> textReplaceStrategies = new List<TextReplaceStrategy>();

        [Header("メッセージ送り時の効果音")]
        [SerializeField]
        private AudioClip textSound;

        public async UniTask<MessageResult> ShowMessageAsync(
            MessageText message,
            MessageConfig config,
            CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            config = SetDefaultConfig(config);
            message = ReplaceTexts(message);

            await OpenFrameAsync(message, config, cToken);
            await RenderTextAsync(message, config, cToken);

            await OpenChoicesFrameAsync(message, config, cToken);
            await RenderChoicesTextsAsync(message, config, cToken);

            MessageResult result = await WaitForCloseAsync(message, config, cToken);

            await CloseFrameAsync(message, config, cToken);

            return result;
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

        protected abstract UniTask OpenFrameAsync(
            MessageText message,
            MessageConfig config,
            CancellationToken cToken);

        protected abstract UniTask RenderTextAsync(
            MessageText message,
            MessageConfig config,
            CancellationToken cToken);

        protected abstract UniTask OpenChoicesFrameAsync(
            MessageText message,
            MessageConfig config,
            CancellationToken cToken);

        protected abstract UniTask RenderChoicesTextsAsync(
            MessageText message,
            MessageConfig config,
            CancellationToken cToken);

        protected abstract UniTask<MessageResult> WaitForCloseAsync(
            MessageText message,
            MessageConfig config,
            CancellationToken cToken);

        protected abstract UniTask CloseFrameAsync(
            MessageText message,
            MessageConfig config,
            CancellationToken cToken);
    }
}
