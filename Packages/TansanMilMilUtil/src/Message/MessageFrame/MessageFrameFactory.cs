using UnityEngine;

namespace TansanMilMil.Util
{
    public class MessageFrameFactory : IMessageFrameFactory
    {
        private readonly GameObject messageFramePrefab;
        private readonly Transform baseObject;

        public MessageFrameFactory(GameObject messageFramePrefab, Transform baseObject)
        {
            this.messageFramePrefab = messageFramePrefab;
            this.baseObject = baseObject;
        }

        public IMessageFrame CreateMessageFrame()
        {
            if (messageFramePrefab == null)
            {
                Debug.LogError("MessageFramePrefab is not set.");
                return null;
            }

            GameObject messageObj = Object.Instantiate(messageFramePrefab, baseObject);
            MessageFrameBase frame = messageObj.GetComponent<MessageFrameBase>();

            if (frame == null)
            {
                Debug.LogError("MessageFrameBase component not found on the prefab.");
                Object.Destroy(messageObj);
                return null;
            }

            return frame;
        }
    }
}
