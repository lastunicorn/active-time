using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.Persistence;
using Moq;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class RecorderInitialTests
    {
        private Recorder2 recorder;
        private Mock<ITimeRecordRepository> recordRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            recordRepositoryMock = new Mock<ITimeRecordRepository>();
            recorder = new Recorder2(recordRepositoryMock.Object);
        }

        [Test]
        public void TestConstructorOk()
        {
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNull()
        {
            new Recorder2(null);
        }

        //[Test]
        //public void TestInitial_CurrentRecord()
        //{
        //    Assert.That(recorder.CurrentRecord, Is.Null);
        //}

        [Test]
        public void TestInitial_State()
        {
            Assert.That(recorder.State, Is.EqualTo(RecorderState.Stopped));
        }

        [Test]
        public void TestInitial_GetTimeFromLastStop()
        {
            TimeSpan? actualTime = recorder.GetTimeFromLastStop();

            Assert.That(actualTime, Is.Null);
        }
    }
}
