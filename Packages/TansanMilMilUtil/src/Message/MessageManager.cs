using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class MessageManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject messageFramePrefab;

        private IMessageService messageService;
        private IMessageFrameFactory frameFactory;

        private void Awake()
        {
            InitializeServices();
        }

        private void InitializeServices()
        {
            frameFactory = new MessageFrameFactory(messageFramePrefab, transform);
            messageService = new MessageService(frameFactory);
        }

        public async UniTask<MessageResult> MessageAsync(MessageText message, MessageConfig config, CancellationToken cToken)
        {
            return await messageService.ShowMessageAsync(message, config, cToken);
        }
    }
}
