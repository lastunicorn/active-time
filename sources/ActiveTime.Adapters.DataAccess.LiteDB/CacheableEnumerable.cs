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

using System.Collections;

namespace DustInTheWind.ActiveTime.Adapters.DataAccess.LiteDB;

internal class CacheableEnumerable<T> : IEnumerable<T>
{
    private readonly DataCache dataCache;
    private readonly IEnumerable<T> enumerable;

    public CacheableEnumerable(DataCache dataCache, IEnumerable<T> enumerable)
    {
        this.dataCache = dataCache ?? throw new ArgumentNullException(nameof(dataCache));
        this.enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new CacheableEnumerator(dataCache, enumerable);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed class CacheableEnumerator : IEnumerator<T>
    {
        private readonly DataCache dataCache;
        private readonly IEnumerable<T> enumerable;
        private IEnumerator<T> enumerator;
        private bool isAddedToCache;

        public T Current
        {
            get
            {
                if (enumerator == null)
                    return default;

                if (!isAddedToCache)
                {
                    // todo: if item exists, it should not be added.
                    dataCache.Add(enumerator.Current);
                    isAddedToCache = true;
                }

                return enumerator.Current;
            }
        }

        object IEnumerator.Current => Current;

        public CacheableEnumerator(DataCache dataCache, IEnumerable<T> enumerable)
        {
            this.dataCache = dataCache ?? throw new ArgumentNullException(nameof(dataCache));
            this.enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
        }

        public bool MoveNext()
        {
            enumerator ??= enumerable.GetEnumerator();
            bool success = enumerator.MoveNext();

            if (success)
                isAddedToCache = false;

            return success;
        }

        public void Reset()
        {
            enumerator ??= enumerable.GetEnumerator();
            enumerator.Reset();

            isAddedToCache = false;
        }

        public void Dispose()
        {
            enumerator?.Dispose();
        }
    }
}