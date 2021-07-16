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
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using Moq;
using NUnit.Framework;
using DayRecord = DustInTheWind.ActiveTime.Common.DayRecord;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels.CommentsViewModelTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<IDayCommentRepository> dayCommentRepositoryMock;
        private CurrentDay currentDay;

        [SetUp]
        public void SetUp()
        {
            dayCommentRepositoryMock = new Mock<IDayCommentRepository>();

            Mock<IUnitOfWorkFactory> unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IRecorderService> recorderService = new Mock<IRecorderService>();
            Mock<IStatusInfoService> statusInfoService = new Mock<IStatusInfoService>();
            currentDay = new CurrentDay(unitOfWorkFactory.Object, logger.Object, recorderService.Object, statusInfoService.Object);
        }

        [Test]
        public void throws_if_currentDayComment_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(null));
        }

        [Test]
        public void successfully_instantiated()
        {
            new CommentsViewModel(currentDay);
        }

        [Test]
        public void Constructor_clears_Comment_if_Date_from_stateService_is_null()
        {
            currentDay.Date = null;

            CommentsViewModel viewModel = new CommentsViewModel(currentDay);

            Assert.That(viewModel.Comment, Is.Null);
        }

        [Test]
        public void Constructor_updates_CurrentDayComment()
        {
            DateTime date = new DateTime(2011, 06, 13);
            currentDay.Date = date;

            new CommentsViewModel(currentDay);

            dayCommentRepositoryMock.VerifyAll();
        }

        [Test]
        public void Constructor_clears_Comment_if_retrieved_DayComment_is_null()
        {
            DateTime date = new DateTime(2011, 06, 13);
            currentDay.Date = date;
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(null as DayRecord);

            CommentsViewModel viewModel = new CommentsViewModel(currentDay);

            Assert.That(viewModel.Comment, Is.Null);
        }
    }
}