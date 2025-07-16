using UnityEngine;

namespace TansanMilMil.Util
{
    public class MessageConfig
    {
        public const float InfiniteTime = -1;
        public enum MessageFrameStyle
        {
            Default = 0,
        }

        public float displayTimeUntilHide { get; set; } = InfiniteTime;
        public int textSoundIntervalMS { get; set; } = 200;
        public AudioClip textSound { get; set; }
        public Vector3? frameCanvasPos { get; set; }
        public MessageFrameStyle frameStyle { get; set; } = MessageFrameStyle.Default;
        public MessageFrameStyle choicesFrameStyle { get; set; } = MessageFrameStyle.Default;
    }
}
