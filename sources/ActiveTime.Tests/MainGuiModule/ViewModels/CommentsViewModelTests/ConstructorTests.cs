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
    public class ConstructorTests
    {
        Mock<IStateService> stateServiceMock;
        Mock<IRegionManager> regionManagerMock;
        Mock<IDayCommentRepository> dayCommentRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            stateServiceMock = new Mock<IStateService>();
            regionManagerMock = new Mock<IRegionManager>();
            dayCommentRepositoryMock = new Mock<IDayCommentRepository>();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void throws_if_stateService_is_null()
        {
            new CommentsViewModel(null, regionManagerMock.Object, dayCommentRepositoryMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void throws_if_regionManager_is_null()
        {
            new CommentsViewModel(stateServiceMock.Object, null, dayCommentRepositoryMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void throws_if_repository_is_null()
        {
            new CommentsViewModel(stateServiceMock.Object, regionManagerMock.Object, null);
        }

        [Test]
        public void successfully_instantiated()
        {
            new CommentsViewModel(stateServiceMock.Object, regionManagerMock.Object, dayCommentRepositoryMock.Object);
        }

        [Test]
        public void sets_initial_Date_value_from_stateService()
        {
            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);

            CommentsViewModel viewModel = CreateNewViewModel();

            stateServiceMock.VerifyAll();
            Assert.That(viewModel.Date, Is.EqualTo(date));
        }

        [Test]
        public void clears_Date_if_Date_from_stateService_is_null()
        {
            stateServiceMock.Setup(x => x.CurrentDate).Returns(null as DateTime?);

            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.Date, Is.Null);
        }

        [Test]
        public void Constructor_clears_Comment_if_Date_from_stateService_is_null()
        {
            stateServiceMock.Setup(x => x.CurrentDate).Returns(null as DateTime?);

            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.Comment, Is.Null);
        }

        [Test]
        public void Constructor_retrieves_DayComment_from_repository_if_date_is_not_null()
        {
            DateTime date = new DateTime(2011, 06, 13);

            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(null as DayComment);

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

        [Test]
        public void Constructor_sets_Comment_from_retrieved_DayComment()
        {
            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);
            DayComment dayComment = new DayComment
            {
                Id = 10,
                Date = date,
                Comment = "some comment"
            };
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(dayComment);

            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.Comment, Is.EqualTo(dayComment.Comment));
        }

        private CommentsViewModel CreateNewViewModel()
        {
            return new CommentsViewModel(stateServiceMock.Object, regionManagerMock.Object, dayCommentRepositoryMock.Object);
        }
    }
}