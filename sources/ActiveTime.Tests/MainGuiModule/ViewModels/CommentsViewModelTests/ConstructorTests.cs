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
using DustInTheWind.ActiveTime.Services;
using DustInTheWind.ActiveTime.ViewModels;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels.CommentsViewModelTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<IStateService> stateServiceMock;
        private Mock<IDayCommentRepository> dayCommentRepositoryMock;
        private Mock<ICurrentDayComment> currentDayComment;

        [SetUp]
        public void SetUp()
        {
            stateServiceMock = new Mock<IStateService>();
            dayCommentRepositoryMock = new Mock<IDayCommentRepository>();
            currentDayComment = new Mock<ICurrentDayComment>();
        }

        [Test]
        public void throws_if_stateService_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(null, currentDayComment.Object));
        }

        [Test]
        public void throws_if_currentDayComment_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(stateServiceMock.Object, null));
        }

        [Test]
        public void successfully_instantiated()
        {
            new CommentsViewModel(stateServiceMock.Object, currentDayComment.Object);
        }

        [Test]
        public void Constructor_clears_Comment_if_Date_from_stateService_is_null()
        {
            stateServiceMock.Setup(x => x.CurrentDate).Returns(null as DateTime?);

            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.Comment, Is.Null);
        }

        [Test]
        public void Constructor_updates_CurrentDayComment()
        {
            DateTime date = new DateTime(2011, 06, 13);

            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);
            currentDayComment.Setup(x => x.Update());

            CreateNewViewModel();

            dayCommentRepositoryMock.VerifyAll();
        }

        [Test]
        public void Constructor_clears_Comment_if_retrieved_DayComment_is_null()
        {
            DateTime date = new DateTime(2011, 06, 13);

            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(null as DayComment);

            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.Comment, Is.Null);
        }

        private CommentsViewModel CreateNewViewModel()
        {
            return new CommentsViewModel(stateServiceMock.Object, currentDayComment.Object);
        }
    }
}