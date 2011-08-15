using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests.DayCommentRepositoryTests
{
    [TestFixture]
    public class DayCommentRepositoryGetByDateTests : RepositoryTestsBase
    {
        private DayCommentRepository dayCommentRepository;
        private DayComment dayComment1;
        private DayComment dayComment2;

        protected override void OnSetUp()
        {
            base.OnSetUp();

            dayCommentRepository = new DayCommentRepository(CurrentSession);

            dayComment1 = new DayComment
            {
                Date = new DateTime(2000, 06, 13),
                Comment = "some comment"
            };

            CurrentSession.Save(dayComment1);

            dayComment2 = new DayComment
            {
                Date = new DateTime(1990, 03, 05),
                Comment = "some comment"
            };

            CurrentSession.Save(dayComment2);
        }

        [Test]
        public void TestGetByDate_First()
        {
            DayComment actualDayComment = dayCommentRepository.GetByDate(new DateTime(2000, 06, 13));

            DayCommentAssertUtil.AssertAreEquals(dayComment1, actualDayComment);
        }

        [Test]
        public void TestGetByDate_Second()
        {
            DayComment actualDayComment = dayCommentRepository.GetByDate(new DateTime(1990, 03, 05));

            DayCommentAssertUtil.AssertAreEquals(dayComment2, actualDayComment);
        }

        [Test]
        public void TestGetByDate_Inexistent()
        {
            DayComment actualDayComment = dayCommentRepository.GetByDate(new DateTime(1980, 04, 01));

            Assert.That(actualDayComment, Is.Null);
        }
    }
}
