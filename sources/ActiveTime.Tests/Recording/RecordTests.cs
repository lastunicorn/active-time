using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;
using DustInTheWind.ActiveTime.Recording;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
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
