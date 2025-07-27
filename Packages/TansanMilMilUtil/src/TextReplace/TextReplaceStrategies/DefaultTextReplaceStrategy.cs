using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireInitializeSingleton]
    public class DefaultTextReplaceStrategy : Singleton<DefaultTextReplaceStrategy>
    {
        private List<TextReplaceStrategy> strategies = new List<TextReplaceStrategy>();

        public void Initialize(List<TextReplaceStrategy> strategies)
        {
            this.strategies = strategies;
        }

        public IReadOnlyCollection<TextReplaceStrategy> GetDefaultStrategies()
        {
            if (strategies == null || strategies.Count == 0)
            {
                Debug.LogError("Default text replace strategies are not initialized. Please call Initialize() before using GetDefaultStrategies().");
                return new List<TextReplaceStrategy>();
            }

            return strategies.AsReadOnly();
        }
    }
}
