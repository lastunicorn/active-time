// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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

using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests.TimeRecordRepositoryTests
{
    internal static class TimeRecordAssertUtil
    {
        public static void AssertAreEquals(TimeRecord expected, TimeRecord actual)
        {
            if (!AreEquals(expected, actual))
                Assert.Fail("The two TimeRecord objects are not equal.");
        }

        public static bool AreEquals(TimeRecord a, TimeRecord b)
        {
            return a != null && b != null && a.Id == b.Id && a.Date == b.Date && a.StartTime == b.StartTime && a.EndTime == b.EndTime && a.RecordType == b.RecordType;
        }
        
        public static void AssertAreEquivalent(IList<TimeRecord> expected, IList<TimeRecord> actual)
        {
            Assert.That(actual.Count == expected.Count);

            foreach (TimeRecord timeRecord in actual)
            {
                if (expected.First<TimeRecord>(c => AreEquals(c, timeRecord)) == null)
                {
                    Assert.Fail("The lists are not equivalent.");
                }
            }
        }
    }
}
