using System;
using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.Infrastructure
{
    public class EventParameters
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public T Get<T>(string name)
        {
            if (!values.ContainsKey(name))
                throw new ArgumentException("Parameter does not exist.", nameof(name));

            return (T)values[name];
        }

        public void Add(string name, object value)
        {
            values.Add(name, value);
        }
    }
}