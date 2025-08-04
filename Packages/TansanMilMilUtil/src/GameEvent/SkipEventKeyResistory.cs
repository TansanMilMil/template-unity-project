using System;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class SkipEventKeyResistory : Singleton<SkipEventKeyResistory>, ISkipEventKeyResistory, IRequireInitialize<ISkipEventKey>
    {
        private ISkipEventKey skipEventKey;

        public void Initialize(ISkipEventKey skipEventKey)
        {
            this.skipEventKey = skipEventKey;
        }

        public void AssertInitialized()
        {
            if (skipEventKey == null)
            {
                throw new InvalidOperationException("SkipEventKey is not initialized. Please call Initialize() before using this method.");
            }
        }

        public ISkipEventKey GetSkipEventKey()
        {
            AssertInitialized();

            return skipEventKey;
        }
    }
}
