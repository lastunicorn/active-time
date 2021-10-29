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