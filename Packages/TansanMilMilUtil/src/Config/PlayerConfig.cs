using System;
using System.Collections.Generic;
using TansanMilMil.Util;
using UnityEngine;

namespace TansanMilMil.Util
{
    [Serializable]
    public class PlayerConfig
    {
        public float playerEffectAddTimeVal = PlayerEffectAddTimeValNormal;
        public string cultureInfoName = null;
        public int messageWriteTextInterval = MessageWriteTextIntervalNormal;
        /// <summary>
        /// 音量をdBで指定。0～-80まで。
        /// </summary>
        public float bgmVolume = 0;
        /// <summary>
        /// 音量をdBで指定。0～-80まで。
        /// </summary>
        public float soundVolume = 0;

        public enum PlayerEffectAddTimeType
        {
            Slow = -1,
            Normal = 0,
            Fast = 1,
        }
        public enum MessageWriteTextIntervalType
        {
            Slow = -1,
            Normal = 0,
            Fast = 1,
        }
        public const float PlayerEffectAddTimeValSlow = 0f;
        public const float PlayerEffectAddTimeValNormal = 0.2f;
        public const float PlayerEffectAddTimeValFast = 0.6f;
        public const int MessageWriteTextIntervalSlow = 50;
        public const int MessageWriteTextIntervalNormal = 15;
        public const int MessageWriteTextIntervalFast = 5;
        public static readonly Dictionary<PlayerEffectAddTimeType, float> PlayerEffectAddTimeValDict = new Dictionary<PlayerEffectAddTimeType, float>()
            {
                { PlayerEffectAddTimeType.Slow, PlayerEffectAddTimeValSlow },
                { PlayerEffectAddTimeType.Normal, PlayerEffectAddTimeValNormal },
                { PlayerEffectAddTimeType.Fast, PlayerEffectAddTimeValFast },
            };
        public static readonly Dictionary<MessageWriteTextIntervalType, int> MessageWriteTextIntervalDict = new Dictionary<MessageWriteTextIntervalType, int>()
            {
                { MessageWriteTextIntervalType.Slow, MessageWriteTextIntervalSlow },
                { MessageWriteTextIntervalType.Normal, MessageWriteTextIntervalNormal },
                { MessageWriteTextIntervalType.Fast, MessageWriteTextIntervalFast },
            };
    }
}
