// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using DustInTheWind.ActiveTime.Recording;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Common.Recording
{
    [TestFixture]
    public class RecordTests
    {
        private Record record;

        [SetUp]
        public void SetUp()
        {
            record = new Record(new DateTime(2000, 06, 13), new TimeSpan(1, 30, 20), new TimeSpan(12, 15, 30));
        }

        [Test]
        public void TestGetStartDateTime()
        {
            DateTime actualValue = record.GetStartDateTime();
            DateTime expectedValue = new DateTime(2000, 06, 13, 1, 30, 20);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Test]
        public void TestGetEndDateTime()
        {
            DateTime actualValue = record.GetEndDateTime();
            DateTime expectedValue = new DateTime(2000, 06, 13, 12, 15, 30);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }
    }
}
