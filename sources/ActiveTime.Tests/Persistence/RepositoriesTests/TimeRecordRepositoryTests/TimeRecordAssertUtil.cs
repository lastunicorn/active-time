using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
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
