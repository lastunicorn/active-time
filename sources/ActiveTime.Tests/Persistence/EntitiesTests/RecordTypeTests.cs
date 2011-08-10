using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.Entities
{
    [TestFixture]
    public class RecordTypeTests
    {
        [Test]
        public void TestNormalValue()
        {
            Assert.That((int)RecordType.Normal, Is.EqualTo(0));
        }

        [Test]
        public void TestCreatedValue()
        {
            Assert.That((int)RecordType.Created, Is.EqualTo(1));
        }
    }
}
