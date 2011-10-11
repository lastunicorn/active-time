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

using System;
using DustInTheWind.ActiveTime.Common.Persistence;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.EntitiesTests
{
    [TestFixture]
    public class DayCommentEqualsTests
    {
        #region Equals

        [Test]
        public void TestEqualsOk()
        {
            DayComment dayComment1 = BuildNewDayComment();
            DayComment dayComment2 = BuildNewDayComment();

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentId()
        {
            DayComment dayComment1 = BuildNewDayComment();
            DayComment dayComment2 = BuildNewDayComment();
            dayComment2.Id = 10;

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentDate()
        {
            DayComment dayComment1 = BuildNewDayComment();
            DayComment dayComment2 = BuildNewDayComment();
            dayComment2.Date = new DateTime(2011, 03, 05);

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_DifferentComment()
        {
            DayComment dayComment1 = BuildNewDayComment();
            DayComment dayComment2 = BuildNewDayComment();
            dayComment2.Comment = "some different comment";

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_AllDifferent()
        {
            DayComment dayComment1 = BuildNewDayComment();
            DayComment dayComment2 = BuildNewDayComment();
            dayComment2.Id = 10;
            dayComment2.Date = new DateTime(2011, 03, 05);
            dayComment2.Comment = "some different comment";

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_AllDifferentButBusinessKey()
        {
            DayComment dayComment1 = BuildNewDayComment();
            DayComment dayComment2 = BuildNewDayComment();
            dayComment2.Id = 10;
            dayComment2.Comment = "some different comment";

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }

        #endregion

        #region Utils

        private DayComment BuildNewDayComment()
        {
            return new DayComment
            {
                Id = 5,
                Date = new DateTime(2000, 06, 13),
                Comment = "some comment"
            };
        }

        #endregion
    }
}
