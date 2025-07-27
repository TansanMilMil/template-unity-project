using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    [RequireInitializeSingleton]
    public class PauseEventsRegistry : Singleton<PauseEventsRegistry>, IPauseEventsRegistry
    {
        private readonly List<IPauseEvents> pauseEvents = new List<IPauseEvents>();

        public void Initialize(IPauseEvents pauseEvent)
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
                Debug.LogError("No pause events registered. Please call Initialize() before using FireOnPauseEvents().");
                return;
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
                Debug.LogError("No resume events registered. Please call Initialize() before using FireOnResumeEvents().");
                return;
            }

            foreach (var pauseEvent in pauseEvents)
            {
                pauseEvent.OnResume();
            }
        }
    }
}
