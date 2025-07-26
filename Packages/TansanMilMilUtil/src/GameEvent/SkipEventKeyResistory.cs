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
            return skipEventKey;
        }
    }
}
