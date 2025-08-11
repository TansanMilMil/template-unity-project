using System.Collections.Generic;
using TansanMilMil.Util;
using UnityEngine;

namespace TemplateUnityProject
{
    public static class DefaultTextReplaceStrategyInitializer
    {
        public static void Initialize()
        {
            var strategies = new List<TextReplaceStrategy>
            {
                new PlayerNameReplaceStrategy(),
            };

            DefaultTextReplaceStrategy.GetInstance().Initialize(strategies);

            Debug.Log("DefaultTextReplaceStrategy initialized with project-specific strategies");
        }
    }

    public class PlayerNameReplaceStrategy : TextReplaceStrategy
    {
        public override string Replace(string text)
        {
            return text.Replace("{PlayerName}", PlayerPrefs.GetString("PlayerName", "Player"));
        }
    }
}
