// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Comments.PresentComments;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Presentation.CalendarArea;
using DustInTheWind.ActiveTime.Presentation.Commands;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Presentation.ViewModels.CommentsViewModelTests;

[TestFixture]
public class PropertyChangedTests
{
    private CommentsViewModel viewModel;

    [SetUp]
    public void SetUp()
    {
        Mock<IRequestBus> requestBus = new();

        PresentCommentsResponse presentCommentsResponse = new()
        {
            Comments = "ha ha ha"
        };
        requestBus
            .Setup(x => x.Send(It.IsAny<PresentCommentsRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(presentCommentsResponse));

        EventBus eventBus = new();
        ResetCommentsCommand resetCommentsCommand = new(requestBus.Object, eventBus);
        SaveCommentsCommand saveCommentsCommand = new(requestBus.Object, eventBus);

        viewModel = new CommentsViewModel(requestBus.Object, eventBus, resetCommentsCommand, saveCommentsCommand);
    }

    #region CommentTextWrap Property

    [Test]
    public void CommentTextWrap_is_initially_true()
    {
        Assert.That(viewModel.CommentTextWrap, Is.True);
    }

    [Test]
    public void CommentTextWrap_raises_PropertyChanged_event()
    {
        bool eventWasCalled = false;
        viewModel.PropertyChanged += (s, e) => { eventWasCalled = true; };

        viewModel.CommentTextWrap = false;

        Assert.That(eventWasCalled, Is.True);
    }

    [Test]
    public void CommentTextWrap_raises_PropertyChanged_event_with_correct_PropertyName()
    {
        string propertyName = null;
        viewModel.PropertyChanged += (s, e) => { propertyName = e.PropertyName; };

        viewModel.CommentTextWrap = false;

        Assert.That(propertyName, Is.EqualTo(GetNameOfMember(() => viewModel.CommentTextWrap)));
    }

    #endregion

    #region Comments Property

    [Test]
    public void Comment_raises_PropertyChanged_event()
    {
        bool eventWasCalled = false;
        viewModel.PropertyChanged += (s, e) => { eventWasCalled = true; };

        viewModel.Comments = "some comment";

        Assert.That(eventWasCalled, Is.True);
    }

    [Test]
    public void Comment_raises_PropertyChanged_event_with_correct_PropertyName()
    {
        string propertyName = null;
        viewModel.PropertyChanged += (s, e) => { propertyName = e.PropertyName; };

        viewModel.Comments = "some comment";

        Assert.That(propertyName, Is.EqualTo(GetNameOfMember(() => viewModel.Comments)));
    }

    #endregion

    private static string GetNameOfMember<T>(Expression<Func<T>> action)
    {
        return ((MemberExpression)action.Body).Member.Name;
    }
}