//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using Moq;
//using DustInTheWind.ActiveTime.Persistence.Repositories;
//using System.Linq.Expressions;
//using DustInTheWind.ActiveTime.Persistence.Entities;

//namespace DustInTheWind.ActiveTime.UnitTests.UI
//{
//    [TestFixture]
//    public class CommentsViewModelTests
//    {
//        #region Constructor

//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void Constructor_throws_if_repository_is_null()
//        {
//            CommentsViewModel viewModel = new CommentsViewModel(null);
//        }

//        [Test]
//        public void Constructor_Ok()
//        {
//            Mock<IDayCommentRepository> dayCommentRepository = new Mock<IDayCommentRepository>();
//            CommentsViewModel viewModel = new CommentsViewModel(dayCommentRepository.Object);
//        }

//        #endregion

//        #region Date

//        [Test]
//        public void Date_initially_is_current_date()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            Assert.That(viewModel.Date, Is.EqualTo(DateTime.Today));
//        }

//        [Test]
//        public void Date_raises_PropertyChanged_event()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            bool eventWasCalled = false;
//            viewModel.PropertyChanged += (s, e) => { eventWasCalled = true; };

//            viewModel.Date = new DateTime(2000, 06, 13);

//            Assert.That(eventWasCalled, Is.True);
//        }

//        [Test]
//        public void Date_raises_PropertyChanged_event_with_correct_PropertyName()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            string propertyName = null;
//            viewModel.PropertyChanged += (s, e) => { propertyName = e.PropertyName; };

//            viewModel.Date = new DateTime(2000, 06, 13);

//            Assert.That(propertyName, Is.EqualTo(GetNameOfMember(() => viewModel.Date)));
//        }

//        #endregion

//        #region CommentTextWrap

//        [Test]
//        public void CommentTextWrap_is_initially_true()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            Assert.That(viewModel.CommentTextWrap, Is.True);
//        }

//        [Test]
//        public void CommentTextWrap_raises_PropertyChanged_event()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            bool eventWasCalled = false;
//            viewModel.PropertyChanged += (s, e) => { eventWasCalled = true; };

//            viewModel.CommentTextWrap = false;

//            Assert.That(eventWasCalled, Is.True);
//        }

//        [Test]
//        public void CommentTextWrap_raises_PropertyChanged_event_with_correct_PropertyName()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            string propertyName = null;
//            viewModel.PropertyChanged += (s, e) => { propertyName = e.PropertyName; };

//            viewModel.CommentTextWrap = false;

//            Assert.That(propertyName, Is.EqualTo(GetNameOfMember(() => viewModel.CommentTextWrap)));
//        }

//        #endregion

//        #region CommentTextWrap

//        [Test]
//        public void Comment_is_initially_empty()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            Assert.That(viewModel.Comment, Is.Null);
//        }

//        [Test]
//        public void Comment_raises_PropertyChanged_event()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            bool eventWasCalled = false;
//            viewModel.PropertyChanged += (s, e) => { eventWasCalled = true; };

//            viewModel.Comment = "some comment";

//            Assert.That(eventWasCalled, Is.True);
//        }

//        [Test]
//        public void Comment_raises_PropertyChanged_event_with_correct_PropertyName()
//        {
//            CommentsViewModel viewModel = CreateNewViewModel();

//            string propertyName = null;
//            viewModel.PropertyChanged += (s, e) => { propertyName = e.PropertyName; };

//            viewModel.Comment = "some comment";

//            Assert.That(propertyName, Is.EqualTo(GetNameOfMember(() => viewModel.Comment)));
//        }

//        #endregion

//        #region WindowLoaded

//        [Test]
//        public void WindowLoaded_initializes_Comment_value()
//        {
//            DayComment dayComment = BuildNewDayComment();
//            Mock<IDayCommentRepository> dayCommentRepository = new Mock<IDayCommentRepository>();
//            dayCommentRepository.Setup(x => x.GetByDate(dayComment.Date)).Returns(dayComment);

//            CommentsViewModel viewModel = new CommentsViewModel(dayCommentRepository.Object);
//            viewModel.WindowLoaded();

//            Assert.That(viewModel.Comment, Is.EqualTo(dayComment.Comment));
//        }

//        [Test]
//        public void WindowLoaded_initializes_with_empty_comment_if_no_DayComment_exists()
//        {
//            Mock<IDayCommentRepository> dayCommentRepository = new Mock<IDayCommentRepository>();
//            dayCommentRepository.Setup(x => x.GetByDate(It.IsAny<DateTime>())).Returns(null as DayComment);

//            CommentsViewModel viewModel = new CommentsViewModel(dayCommentRepository.Object);
//            viewModel.WindowLoaded();

//            Assert.That(viewModel.Comment, Is.EqualTo(string.Empty));
//        }

//        [Test]
//        public void WindowLoaded_not_throws_when_exception()
//        {
//            Mock<IDayCommentRepository> dayCommentRepository = new Mock<IDayCommentRepository>();
//            dayCommentRepository.Setup(x => x.GetByDate(It.IsAny<DateTime>())).Throws(new Exception());

//            CommentsViewModel viewModel = new CommentsViewModel(dayCommentRepository.Object);
//            viewModel.WindowLoaded();
//        }

//        #endregion

//        #region CancelButtonClicked

//        //[Test]
//        //public void CancelButtonClicked_

//        #endregion


//        #region Utils

//        private static CommentsViewModel CreateNewViewModel()
//        {
//            Mock<IDayCommentRepository> dayCommentRepository = new Mock<IDayCommentRepository>();
//            CommentsViewModel viewModel = new CommentsViewModel(dayCommentRepository.Object);
//            return viewModel;
//        }

//        private string GetNameOfMember<T>(Expression<Func<T>> action)
//        {
//            return ((MemberExpression)action.Body).Member.Name;
//        }

//        private DayComment BuildNewDayComment()
//        {
//            return new DayComment() { Id = 10, Date = DateTime.Today, Comment = "some comment" };
//        }

//        #endregion
//    }
//}