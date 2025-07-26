using System.Collections.Generic;

namespace TansanMilMil.Util
{
    public class PauseEventsRegistry : Singleton<PauseEventsRegistry>, IPauseEventsRegistry
    {
        private readonly List<IPauseEvents> pauseEvents = new List<IPauseEvents>();

        public void Register(IPauseEvents pauseEvent)
        {
            if (!pauseEvents.Contains(pauseEvent))
            {
                pauseEvents.Add(pauseEvent);
            }
        }

        public void FireOnPauseEvents()
        {
            if (pauseEvents.Count == 0)
            {
                throw new System.InvalidOperationException("No pause events registered.");
            }

            foreach (var pauseEvent in pauseEvents)
            {
                pauseEvent.OnPause();
            }
        }

        public void FireOnResumeEvents()
        {
            if (pauseEvents.Count == 0)
            {
                throw new System.InvalidOperationException("No resume events registered.");
            }

            foreach (var pauseEvent in pauseEvents)
            {
                pauseEvent.OnResume();
            }
        }
    }
}
