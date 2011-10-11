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
using DustInTheWind.ActiveTime.Common.Persistence;
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
