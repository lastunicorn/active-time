using System;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class ScribTests
    {
        private Mock<ITimeRecordRepository> repositoryMock;
        private Mock<ITimeProvider> timeProviderMock;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<ITimeRecordRepository>();
            timeProviderMock = new Mock<ITimeProvider>();
        }

        private Scrib CreateRecorder()
        {
            return new Scrib(repositoryMock.Object, timeProviderMock.Object);
        }

        [Test]
        public void Constructor()
        {
            Scrib recorder = CreateRecorder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_with_null_repository()
        {
            Scrib recorder = new Scrib(null, timeProviderMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_with_null_timeProvider()
        {
            Scrib recorder = new Scrib(repositoryMock.Object, null);
        }

        [Test]
        public void CreateNewRecord_calls_timeProvider()
        {
            Scrib recorder = CreateRecorder();
            DateTime now = DateTime.Now;
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(now);
            
            recorder.CreateNewRecord();

            timeProviderMock.VerifyAll();
        }

        [Test]
        public void CreateNewRecord_saves_a_new_record_in_repository()
        {
            Scrib recorder = CreateRecorder();
            DateTime now = DateTime.Now;
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(now);
            repositoryMock.Setup(x => x.Add(It.Is<TimeRecord>(r => r.Id == 0 &&
                                                                   r.RecordType == TimeRecordType.Normal &&
                                                                   r.Date == now.Date &&
                                                                   (r.StartTime - now.TimeOfDay) < TimeSpan.FromSeconds(1) &&
                                                                   r.EndTime == r.StartTime)));
            recorder.CreateNewRecord();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void CreateNewRecord_called_twice_adds_new_record()
        {
            Scrib recorder = CreateRecorder();

            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));

            recorder.CreateNewRecord();
            recorder.CreateNewRecord();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void Stamp_creates_new_record_if_not_exist()
        {
            Scrib recorder = CreateRecorder();

            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            repositoryMock.Setup(x => x.Update(It.IsAny<TimeRecord>()));

            recorder.Stamp();

            repositoryMock.VerifyAll();
        }
    }
}
