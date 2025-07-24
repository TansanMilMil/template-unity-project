using System.Collections.Generic;

namespace TansanMilMil.Util
{
    public static class PauseEventsRegistry
    {
        private static readonly List<IPauseEvents> pauseEvents = new List<IPauseEvents>();

        public static void Register(IPauseEvents pauseEvent)
        {
            if (!pauseEvents.Contains(pauseEvent))
            {
                pauseEvents.Add(pauseEvent);
            }
        }

        public static void FireOnPauseEvents()
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

        public static void FireOnResumeEvents()
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
