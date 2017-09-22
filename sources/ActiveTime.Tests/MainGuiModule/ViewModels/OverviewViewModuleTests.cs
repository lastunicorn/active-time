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
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.ViewModels;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels
{
    /// <summary>
    /// Contains unit tests for the initialization of a new <see cref="OverviewViewModuleTests"/> object.
    /// </summary>
    [TestFixture]
    public class OverviewViewModuleTests
    {
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IDayCommentRepository> dayCommentRepository;
        private Mock<ITimeRecordRepository> timeRecordRepository;
        private Mock<ITimeProvider> timeProvider;

        [SetUp]
        public void SetUp()
        {
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            unitOfWork = new Mock<IUnitOfWork>();
            dayCommentRepository = new Mock<IDayCommentRepository>();
            timeRecordRepository = new Mock<ITimeRecordRepository>();

            unitOfWorkFactory
                .Setup(x => x.CreateNew())
                .Returns(unitOfWork.Object);

            unitOfWork
                .Setup(x => x.TimeRecordRepository)
                .Returns(timeRecordRepository.Object);

            unitOfWork
                .Setup(x => x.DayCommentRepository)
                .Returns(dayCommentRepository.Object);

            dayCommentRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<DayComment>());

            timeProvider = new Mock<ITimeProvider>();
        }

        [Test]
        public void LastDay_is_initialized_with_the_current_day_from_timeProvider()
        {
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(new DateTime(1980, 06, 13));

            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);

            Assert.AreEqual(new DateTime(1980, 06, 13), overviewViewModel.LastDay);
        }

        [Test]
        public void FirstDay_is_initialized_with_29_days_before_the_current_day_from_timeProvider()
        {
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(new DateTime(1980, 06, 13));

            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);

            Assert.AreEqual(new DateTime(1980, 05, 15), overviewViewModel.FirstDay);
        }

        [Test]
        public void calls_GetByDate_with_FirstDay_and_LastDay_values()
        {
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(new DateTime(1980, 06, 13));

            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);

            dayCommentRepository.Verify(x => x.GetByDate(overviewViewModel.FirstDay, overviewViewModel.LastDay), Times.Once());
        }

        [Test]
        public void populates_Comments_property()
        {
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(DateTime.Now);

            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);

            Assert.IsNotNull(overviewViewModel.Comments);
        }

        [Test]
        public void populates_Reports_property_with_data_retrieved_from_dayCommentRepository()
        {
            List<DayComment> dayComments = new List<DayComment> { new DayComment(), new DayComment(), new DayComment() };
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(DateTime.Now);
            dayCommentRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(dayComments);

            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);

            Assert.AreEqual(3, overviewViewModel.Reports.Count);
        }

        [Test]
        public void calls_GetByDate_when_FirsDay_is_set()
        {
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(new DateTime(1980, 06, 13));
            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);
            dayCommentRepository.ResetCalls();

            overviewViewModel.FirstDay = new DateTime(2000, 06, 13);

            dayCommentRepository.Verify(x => x.GetByDate(overviewViewModel.FirstDay, overviewViewModel.LastDay), Times.Once());
        }

        [Test]
        public void calls_GetByDate_when_LastDay_is_set()
        {
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(new DateTime(1980, 06, 13));
            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);
            dayCommentRepository.ResetCalls();

            overviewViewModel.LastDay = new DateTime(2000, 06, 13);

            dayCommentRepository.Verify(x => x.GetByDate(overviewViewModel.FirstDay, overviewViewModel.LastDay), Times.Once());
        }

        [Test]
        public void when_setting_FirstDay_PropertyChanged_event_is_raised()
        {
            bool eventWasRaised = false;
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(DateTime.Now);
            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);
            overviewViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "FirstDay")
                    eventWasRaised = true;
            };

            overviewViewModel.FirstDay = new DateTime(2000, 06, 13);

            Assert.IsTrue(eventWasRaised);
        }

        [Test]
        public void when_setting_LastDay_PropertyChanged_event_is_raised()
        {
            bool eventWasRaised = false;
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(DateTime.Now);
            OverviewViewModel overviewViewModel = new OverviewViewModel(unitOfWorkFactory.Object, timeProvider.Object);
            overviewViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "LastDay")
                    eventWasRaised = true;
            };

            overviewViewModel.LastDay = new DateTime(2000, 06, 13);

            Assert.IsTrue(eventWasRaised);
        }
    }
}
