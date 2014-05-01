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

namespace DustInTheWind.ActiveTime.UnitTests.Common.Persistence
{
    [TestFixture]
    public class DayCommentPropertyTests
    {
        private DayComment dayComment;

        [SetUp]
        public void SetUp()
        {
            dayComment = new DayComment();
        }

        [Test]
        public void TestDate_GetSet()
        {
            DateTime date = new DateTime(2011, 06, 13);
            dayComment.Date = date;

            Assert.That(dayComment.Date, Is.EqualTo(date));
        }

        [Test]
        public void TestComment_GetSet()
        {
            string comment = "this is a comment";
            dayComment.Comment = comment;

            Assert.That(dayComment.Comment, Is.SameAs(comment));
        }
    }
}
