using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.EntitiesTests
{
    [TestFixture]
    public class TimeRecordTypeTests
    {
        [Test]
        public void TestNormalValue()
        {
            Assert.That((int)TimeRecordType.Normal, Is.EqualTo(0));
        }

        [Test]
        public void TestCreatedValue()
        {
            Assert.That((int)TimeRecordType.Fake, Is.EqualTo(1));
        }
    }
}
