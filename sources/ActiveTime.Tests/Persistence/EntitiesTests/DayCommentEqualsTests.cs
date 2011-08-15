using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.EntitiesTests
{
    [TestFixture]
    public class DayCommentEqualsTests
    {
        private DayComment dayComment1;
        private DayComment dayComment2;

        [SetUp]
        public void SetUp()
        {
            dayComment1 = new DayComment
            {
                Id = 5,
                Date = new DateTime(2000, 06, 13),
                Comment = "some comment"
            };

            dayComment2 = new DayComment
            {
                Id = 5,
                Date = new DateTime(2000, 06, 13),
                Comment = "some comment"
            };
        }

        [Test]
        public void TestEqualsOk()
        {
            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentId()
        {
            dayComment2.Id = 10;

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentDate()
        {
            dayComment2.Date = new DateTime(2011, 03, 05);

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_DifferentComment()
        {
            dayComment2.Comment = "some different comment";

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_AllDifferent()
        {
            dayComment2.Id = 10;
            dayComment2.Date = new DateTime(2011, 03, 05);
            dayComment2.Comment = "some different comment";

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_AllDifferentButBusinessKey()
        {
            dayComment2.Id = 10;
            dayComment2.Comment = "some different comment";

            bool actualValue = dayComment1.Equals(dayComment2);

            Assert.That(actualValue, Is.True);
        }
    }
}
