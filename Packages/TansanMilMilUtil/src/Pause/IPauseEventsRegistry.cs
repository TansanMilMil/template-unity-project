namespace TansanMilMil.Util
{
    public interface IPauseEventsRegistry
    {
        void Initialize(IPauseEvents pauseEvent);
        void FireOnPauseEvents();
        void FireOnResumeEvents();
    }
}
