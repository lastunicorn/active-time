// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.Infrastructure.EventModel
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