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
using DustInTheWind.ActiveTime.Domain;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Common.Persistence
{
    [TestFixture]
    public class DayCommentPropertyTests
    {
        private DateRecord dateRecord;

        [SetUp]
        public void SetUp()
        {
            dateRecord = new DateRecord();
        }

        [Test]
        public void TestDate_GetSet()
        {
            DateTime date = new DateTime(2011, 06, 13);
            dateRecord.Date = date;

            Assert.That(dateRecord.Date, Is.EqualTo(date));
        }

        [Test]
        public void TestComment_GetSet()
        {
            string comment = "this is a comment";
            dateRecord.Comment = comment;

            Assert.That(dateRecord.Comment, Is.SameAs(comment));
        }
    }
}
