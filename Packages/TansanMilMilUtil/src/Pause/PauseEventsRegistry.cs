using System;
using System.Collections.Generic;
using UnityEngine;

namespace TansanMilMil.Util
{
    public class PauseEventsRegistry : Singleton<PauseEventsRegistry>, IPauseEventsRegistry, IRequireInitialize<IPauseEvents>
    {
        private readonly List<IPauseEvents> pauseEvents = new List<IPauseEvents>();

        public void Initialize(IPauseEvents pauseEvent)
        {
            if (!pauseEvents.Contains(pauseEvent))
            {
                pauseEvents.Add(pauseEvent);
            }
        }

        public void AssertInitialized()
        {
            if (pauseEvents.Count == 0)
            {
                throw new InvalidOperationException("PauseEventsRegistry is not initialized. Please call Initialize() before using this method.");
            }
        }

        public void FireOnPauseEvents()
        {
            AssertInitialized();

            foreach (var pauseEvent in pauseEvents)
            {
                pauseEvent.OnPause();
            }
        }

        public void FireOnResumeEvents()
        {
            AssertInitialized();

            foreach (var pauseEvent in pauseEvents)
            {
                pauseEvent.OnResume();
            }
        }
    }
}
