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
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Services;
using DustInTheWind.ActiveTime.ViewModels;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels.CommentsViewModelTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<IDayCommentRepository> dayCommentRepositoryMock;
        private Mock<ICurrentDay> currentDay;

        [SetUp]
        public void SetUp()
        {
            dayCommentRepositoryMock = new Mock<IDayCommentRepository>();
            currentDay = new Mock<ICurrentDay>();
        }

        [Test]
        public void throws_if_currentDayComment_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(null));
        }

        [Test]
        public void successfully_instantiated()
        {
            new CommentsViewModel(currentDay.Object);
        }

        [Test]
        public void Constructor_clears_Comment_if_Date_from_stateService_is_null()
        {
            currentDay.Setup(x => x.Date).Returns(null as DateTime?);

            CommentsViewModel viewModel = new CommentsViewModel(currentDay.Object);

            Assert.That(viewModel.Comment, Is.Null);
        }

        [Test]
        public void Constructor_updates_CurrentDayComment()
        {
            DateTime date = new DateTime(2011, 06, 13);

            currentDay.Setup(x => x.Date).Returns(date);
            currentDay.Setup(x => x.ReloadComments());

            new CommentsViewModel(currentDay.Object);

            dayCommentRepositoryMock.VerifyAll();
        }

        [Test]
        public void Constructor_clears_Comment_if_retrieved_DayComment_is_null()
        {
            DateTime date = new DateTime(2011, 06, 13);

            currentDay.Setup(x => x.Date).Returns(date);
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(null as DayComment);

            CommentsViewModel viewModel = new CommentsViewModel(currentDay.Object);

            Assert.That(viewModel.Comment, Is.Null);
        }
    }
}