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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using DustInTheWind.ActiveTime.PersistenceModule.Repositories;
using System.Linq.Expressions;
using DustInTheWind.ActiveTime.MainGuiModule.ViewModels;
using DustInTheWind.ActiveTime.Common;
using Microsoft.Practices.Prism.Regions;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.UnitTests.UI
{
    [TestFixture]
    public class CommentsViewModelTests
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

        #region Constructor Parameters

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_stateService_is_null()
        {
            new CommentsViewModel(null, regionManagerMock.Object, dayCommentRepositoryMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_regionManager_is_null()
        {
            new CommentsViewModel(stateServiceMock.Object, null, dayCommentRepositoryMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_repository_is_null()
        {
            new CommentsViewModel(stateServiceMock.Object, regionManagerMock.Object, null);
        }

        [Test]
        public void Constructor_Ok()
        {
            new CommentsViewModel(stateServiceMock.Object, regionManagerMock.Object, dayCommentRepositoryMock.Object);
        }

        #endregion

        #region Constructor

        [Test]
        public void Constructor_reads_Date_from_stateService()
        {
            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);

            CommentsViewModel viewModel = CreateNewViewModel();

            stateServiceMock.VerifyAll();
            Assert.That(viewModel.Date, Is.EqualTo(date));
        }

        [Test]
        public void Constructor_clears_Date_if_Date_from_stateService_is_null()
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
        public void Constructor_clears_Comment_if_retrieved_DayComment_has_wrong_date()
        {
            DateTime date = new DateTime(2011, 06, 13);
            DateTime differentDate = new DateTime(2000, 03, 05);
            DayComment dayComment = new DayComment { Id = 10, Date = differentDate, Comment = "some comment" };

            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(dayComment);

            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.Comment, Is.Null);
        }

        [Test]
        public void Constructor_sets_Comment_from_retrieved_DayComment()
        {
            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);
            DayComment dayComment = new DayComment { Id = 10, Date = date, Comment = "some comment" };
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(dayComment);

            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.Comment, Is.EqualTo(dayComment.Comment));
        }

        #endregion

        #region Date

        [Test]
        public void Date_sets_stateService_CurrentDate()
        {
            DateTime date = new DateTime(2011, 06, 13);
            CommentsViewModel viewModel = CreateNewViewModel();
            stateServiceMock.SetupSet(x => x.CurrentDate = date);

            viewModel.Date = date;

            stateServiceMock.VerifyAll();
        }

        #endregion

        #region StateService -> CurrentDateChanged

        [Test]
        public void StateService_CurrentDateChanged_changes_Date()
        {
            CommentsViewModel viewModel = CreateNewViewModel();

            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);

            stateServiceMock.Raise(x => x.CurrentDateChanged += null, EventArgs.Empty);

            Assert.That(viewModel.Date, Is.EqualTo(date));
        }

        [Test]
        public void StateService_CurrentDateChanged_changes_Comment()
        {
            CommentsViewModel viewModel = CreateNewViewModel();

            DateTime date = new DateTime(2011, 06, 13);
            stateServiceMock.Setup(x => x.CurrentDate).Returns(date);

            DayComment dayComment = new DayComment { Id = 10, Date = date, Comment = "some comment here" };
            dayCommentRepositoryMock.Setup(x => x.GetByDate(date)).Returns(dayComment);

            stateServiceMock.Raise(x => x.CurrentDateChanged += null, EventArgs.Empty);

            Assert.That(viewModel.Comment, Is.EqualTo(dayComment.Comment));
        }

        #endregion

        #region CommentTextWrap

        [Test]
        public void CommentTextWrap_is_initially_true()
        {
            CommentsViewModel viewModel = CreateNewViewModel();

            Assert.That(viewModel.CommentTextWrap, Is.True);
        }

        [Test]
        public void CommentTextWrap_raises_PropertyChanged_event()
        {
            CommentsViewModel viewModel = CreateNewViewModel();

            bool eventWasCalled = false;
            viewModel.PropertyChanged += (s, e) => { eventWasCalled = true; };

            viewModel.CommentTextWrap = false;

            Assert.That(eventWasCalled, Is.True);
        }

        [Test]
        public void CommentTextWrap_raises_PropertyChanged_event_with_correct_PropertyName()
        {
            CommentsViewModel viewModel = CreateNewViewModel();

            string propertyName = null;
            viewModel.PropertyChanged += (s, e) => { propertyName = e.PropertyName; };

            viewModel.CommentTextWrap = false;

            Assert.That(propertyName, Is.EqualTo(GetNameOfMember(() => viewModel.CommentTextWrap)));
        }

        #endregion

        #region Comment

        [Test]
        public void Comment_raises_PropertyChanged_event()
        {
            CommentsViewModel viewModel = CreateNewViewModel();

            bool eventWasCalled = false;
            viewModel.PropertyChanged += (s, e) => { eventWasCalled = true; };

            viewModel.Comment = "some comment";

            Assert.That(eventWasCalled, Is.True);
        }

        [Test]
        public void Comment_raises_PropertyChanged_event_with_correct_PropertyName()
        {
            CommentsViewModel viewModel = CreateNewViewModel();

            string propertyName = null;
            viewModel.PropertyChanged += (s, e) => { propertyName = e.PropertyName; };

            viewModel.Comment = "some comment";

            Assert.That(propertyName, Is.EqualTo(GetNameOfMember(() => viewModel.Comment)));
        }

        #endregion

        #region ButtonBar

        // The ButtonBar is a private instance. Cannot mock, cannot test.

        #endregion

        #region Utils

        private CommentsViewModel CreateNewViewModel()
        {
            return new CommentsViewModel(stateServiceMock.Object, regionManagerMock.Object, dayCommentRepositoryMock.Object);
        }

        private string GetNameOfMember<T>(Expression<Func<T>> action)
        {
            return ((MemberExpression)action.Body).Member.Name;
        }

        #endregion
    }
}