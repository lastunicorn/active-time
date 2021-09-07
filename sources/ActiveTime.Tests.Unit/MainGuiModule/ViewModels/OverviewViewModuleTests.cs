// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.System;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using MediatR;
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
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<IDayCommentRepository> dayCommentRepository;
        private Mock<ITimeRecordRepository> timeRecordRepository;
        private Mock<ISystemClock> systemClock;
        private OverviewViewModel overviewViewModel;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            dayCommentRepository = new Mock<IDayCommentRepository>();
            timeRecordRepository = new Mock<ITimeRecordRepository>();

            unitOfWork
                .Setup(x => x.TimeRecordRepository)
                .Returns(timeRecordRepository.Object);

            unitOfWork
                .Setup(x => x.DayCommentRepository)
                .Returns(dayCommentRepository.Object);

            dayCommentRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<DayRecord>());

            systemClock = new Mock<ISystemClock>();

            Mock<IMediator> mediator = new Mock<IMediator>();
            Mock<ILogger> logger = new Mock<ILogger>();
            overviewViewModel = new OverviewViewModel(systemClock.Object, mediator.Object, logger.Object);
        }

        [Test]
        public void LastDay_is_initialized_with_the_current_day_from_timeProvider()
        {
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(new DateTime(1980, 06, 13));

            Assert.AreEqual(new DateTime(1980, 06, 13), overviewViewModel.LastDay);
        }

        [Test]
        public void FirstDay_is_initialized_with_29_days_before_the_current_day_from_timeProvider()
        {
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(new DateTime(1980, 06, 13));

            Assert.AreEqual(new DateTime(1980, 05, 15), overviewViewModel.FirstDay);
        }

        [Test]
        public void calls_GetByDate_with_FirstDay_and_LastDay_values()
        {
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(new DateTime(1980, 06, 13));

            dayCommentRepository.Verify(x => x.GetByDate(overviewViewModel.FirstDay, overviewViewModel.LastDay), Times.Once());
        }

        [Test]
        public void populates_Comments_property()
        {
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(DateTime.Now);

            Assert.IsNotNull(overviewViewModel.Comments);
        }

        [Test]
        public void calls_GetByDate_when_FirsDay_is_set()
        {
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(new DateTime(1980, 06, 13));
            
            dayCommentRepository.Invocations.Clear();

            overviewViewModel.FirstDay = new DateTime(2000, 06, 13);

            dayCommentRepository.Verify(x => x.GetByDate(overviewViewModel.FirstDay, overviewViewModel.LastDay), Times.Once());
        }

        [Test]
        public void calls_GetByDate_when_LastDay_is_set()
        {
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(new DateTime(1980, 06, 13));
            
            dayCommentRepository.Invocations.Clear();

            overviewViewModel.LastDay = new DateTime(2000, 06, 13);

            dayCommentRepository.Verify(x => x.GetByDate(overviewViewModel.FirstDay, overviewViewModel.LastDay), Times.Once());
        }

        [Test]
        public void when_setting_FirstDay_PropertyChanged_event_is_raised()
        {
            bool eventWasRaised = false;
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(DateTime.Now);
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
            systemClock
                .Setup(x => x.GetCurrentDate())
                .Returns(DateTime.Now);
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