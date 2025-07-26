namespace TansanMilMil.Util
{
    public interface ISkipEventKeyResistory
    {
        void Initialize(ISkipEventKey skipEventKey);

        ISkipEventKey GetSkipEventKey();
    }
}
