using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests.DayCommentRepositoryTests
{
    internal static class DayCommentAssertUtil
    {
        public static void AssertAreEquals(DayComment expected, DayComment actual)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Date, Is.EqualTo(expected.Date));
            Assert.That(actual.Comment, Is.EqualTo(expected.Comment));
        }

        public static void AssertAreEquivalent(IList<DayComment> expectedDayComments, IList<DayComment> actualDayComments)
        {
            Assert.That(actualDayComments.Count == expectedDayComments.Count);

            foreach (DayComment dayComment in actualDayComments)
            {
                if (expectedDayComments.First(c => c.Id == dayComment.Id && c.Date == dayComment.Date && c.Comment == dayComment.Comment) == null)
                {
                    Assert.Fail("The lists are not equivalent.");
                }
            }
        }
    }
}
