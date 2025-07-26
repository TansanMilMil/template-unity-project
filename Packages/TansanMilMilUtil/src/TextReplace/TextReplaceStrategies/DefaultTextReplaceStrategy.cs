using System.Collections.Generic;

namespace TansanMilMil.Util
{
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
                throw new System.InvalidOperationException("Default text replace strategies are not initialized. Please call Initialize() before using this method.");
            }

            return strategies.AsReadOnly();
        }
    }
}
