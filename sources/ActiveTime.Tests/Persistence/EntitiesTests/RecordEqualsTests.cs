using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.Entities
{
    [TestFixture]
    public class RecordEqualsTests
    {
        private Record record1;
        private Record record2;

        [SetUp]
        public void SetUp()
        {
            record1 = new Record
            {
                Id = 5,
                Date = new DateTime(2000, 06, 13),
                RecordType = RecordType.Normal,
                StartTime = new TimeSpan(1, 30, 20),
                EndTime = new TimeSpan(12, 15, 30)
            };

            record2 = new Record
            {
                Id = 5,
                Date = new DateTime(2000, 06, 13),
                RecordType = RecordType.Normal,
                StartTime = new TimeSpan(1, 30, 20),
                EndTime = new TimeSpan(12, 15, 30)
            };
        }

        [Test]
        public void TestEqualsOk()
        {
            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentId()
        {
            record2.Id = 10;

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_DifferentDate()
        {
            record2.Date = new DateTime(2011, 03, 05);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentRecordType()
        {
            record2.RecordType = RecordType.Fake;

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentStartTime()
        {
            record2.StartTime = TimeSpan.FromHours(20);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentEndTime()
        {
            record2.EndTime = TimeSpan.FromHours(20);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_AllDifferent()
        {
            record2.Id = 10;
            record2.Date = new DateTime(2011, 03, 05);
            record2.RecordType = RecordType.Fake;
            record2.StartTime = TimeSpan.FromHours(20);
            record2.EndTime = TimeSpan.FromHours(21);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_AllDifferentButId()
        {
            record2.Date = new DateTime(2011, 03, 05);
            record2.RecordType = RecordType.Fake;
            record2.StartTime = TimeSpan.FromHours(20);
            record2.EndTime = TimeSpan.FromHours(21);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }
    }
}
