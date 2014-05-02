// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.MainGuiModule.ViewModels;
using Microsoft.Practices.Prism.Regions;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels.CommentsViewModelTests
{
    [TestFixture]
    public class DateTests
    {
        Mock<IStateService> stateServiceMock;
        Mock<IRegionManager> regionManagerMock;
        Mock<IDayCommentRepository> dayCommentRepositoryMock;
        private CommentsViewModel viewModel;

        [SetUp]
        public void SetUp()
        {
            stateServiceMock = new Mock<IStateService>();
            regionManagerMock = new Mock<IRegionManager>();
            dayCommentRepositoryMock = new Mock<IDayCommentRepository>();

            viewModel = new CommentsViewModel(stateServiceMock.Object, regionManagerMock.Object, dayCommentRepositoryMock.Object);
        }

        [Test]
        public void Date_sets_stateService_CurrentDate()
        {
            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.SetupSet(x => x.CurrentDate = date);

            viewModel.Date = date;

            stateServiceMock.VerifyAll();
        }

        [Test]
        public void StateService_CurrentDateChanged_changes_Date()
        {
            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);

            stateServiceMock.Raise(x => x.CurrentDateChanged += null, EventArgs.Empty);

            Assert.That(viewModel.Date, Is.EqualTo(date));
        }

        [Test]
        public void StateService_CurrentDateChanged_changes_Comment()
        {
            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);

            DayComment dayComment = new DayComment { Id = 10, Date = date, Comment = "some comment here" };
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(dayComment);

            stateServiceMock.Raise(x => x.CurrentDateChanged += null, EventArgs.Empty);

            Assert.That(viewModel.Comment, Is.EqualTo(dayComment.Comment));
        }
    }
}