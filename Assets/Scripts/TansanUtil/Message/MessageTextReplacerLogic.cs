using TansanMilMil.Util;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class MessageTextReplacerLogic : IMessageTextReplacerLogic
    {
        public string ReplaceText(string text)
        {
            // string playerNamePattern = @"{PlayerStatus.PlayerName}";
            // if (text.Contains(playerNamePattern))
            // {
            //     text = text.Replace(playerNamePattern, PlayerStatus.GetInstance().playerName);
            // }

            return text;
        }
    }
}