using UnityEngine;

namespace TansanMilMil.Util
{
    public class SkipEventKeyResistory : Singleton<SkipEventKeyResistory>, ISkipEventKeyResistory
    {
        private ISkipEventKey skipEventKey;

        public void Initialize(ISkipEventKey skipEventKey)
        {
            this.skipEventKey = skipEventKey;
        }

        public ISkipEventKey GetSkipEventKey()
        {
            if (skipEventKey == null)
            {
                Debug.LogError("SkipEventKey is not initialized. Please call Initialize() before using GetSkipEventKey().");
                return null;
            }

            return skipEventKey;
        }
    }
}
