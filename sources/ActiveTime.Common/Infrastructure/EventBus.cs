using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DustInTheWind.ActiveTime.Common.Infrastructure
{
    public class EventBus
    {
        private readonly Dictionary<string, List<Action<EventParameters>>> eventHandlers = new Dictionary<string, List<Action<EventParameters>>>();

        public void Subscribe(string eventName, Action<EventParameters> eventHandler)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));
            if (eventHandler == null) throw new ArgumentNullException(nameof(eventHandler));

            List<Action<EventParameters>> currentEventHandlers = GetOrCreateHandlersFor(eventName);

            currentEventHandlers.Add(eventHandler);
        }

        public void Unsubscribe(string eventName, Action<EventParameters> eventHandler)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));
            if (eventHandler == null) throw new ArgumentNullException(nameof(eventHandler));

            List<Action<EventParameters>> currentEventHandlers = GetOrCreateHandlersFor(eventName);

            currentEventHandlers.Remove(eventHandler);
        }

        public void Raise(string eventName, EventParameters parameter = null)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));

            List<Action<EventParameters>> currentEventHandlers = GetOrCreateHandlersFor(eventName);

            foreach (Action<EventParameters> currentEventHandler in currentEventHandlers)
                currentEventHandler(parameter);
        }

        private List<Action<EventParameters>> GetOrCreateHandlersFor(string eventName)
        {
            if (eventHandlers.ContainsKey(eventName))
                return eventHandlers[eventName];

            List<Action<EventParameters>> currentEventHandlers = new List<Action<EventParameters>>();
            eventHandlers.Add(eventName, currentEventHandlers);

            return currentEventHandlers;
        }
    }
}