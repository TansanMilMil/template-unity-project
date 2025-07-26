namespace TansanMilMil.Util
{
    public interface IPauseEventsRegistry
    {
        void Register(IPauseEvents pauseEvent);
        void FireOnPauseEvents();
        void FireOnResumeEvents();
    }
}