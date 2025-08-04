using System;
using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class DefaultTextReplaceStrategy : Singleton<DefaultTextReplaceStrategy>, IRequireInitialize<List<TextReplaceStrategy>>
    {
        private List<TextReplaceStrategy> strategies = new List<TextReplaceStrategy>();

        public void Initialize(List<TextReplaceStrategy> strategies)
        {
            this.strategies = strategies;
        }

        public void AssertInitialized()
        {
            if (strategies == null || strategies.Count == 0)
            {
                throw new InvalidOperationException("DefaultTextReplaceStrategy is not initialized. Please call Initialize() before using this method.");
            }
        }

        public IReadOnlyCollection<TextReplaceStrategy> GetDefaultStrategies()
        {
            AssertInitialized();

            return strategies.AsReadOnly();
        }
    }
}
