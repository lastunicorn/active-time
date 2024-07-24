// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

namespace DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;

/// <summary>
/// A collection of subscriber objects grouped by event type.
/// </summary>
internal class SubscribersCollection
{
    private readonly Dictionary<Type, List<object>> subscribersByEvent = new();

    public List<object> GetOrCreateBucket<TEvent>()
    {
        return GetBucket<TEvent>() ?? CreateBucket<TEvent>();
    }

    public List<object> GetBucket<TEvent>()
    {
        return subscribersByEvent.ContainsKey(typeof(TEvent))
            ? subscribersByEvent[typeof(TEvent)]
            : null;
    }

    public List<object> CreateBucket<TEvent>()
    {
        List<object> actions = new();
        subscribersByEvent.Add(typeof(TEvent), actions);
        return actions;
    }
}