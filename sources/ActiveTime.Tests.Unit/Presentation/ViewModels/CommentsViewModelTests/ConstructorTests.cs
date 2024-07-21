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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.UseCases.Comments.PresentComments;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseModel;
using DustInTheWind.ActiveTime.Presentation.CalendarArea;
using DustInTheWind.ActiveTime.Presentation.Commands;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Presentation.ViewModels.CommentsViewModelTests;

[TestFixture]
public class ConstructorTests
{
    private Mock<IRequestBus> requestBus;
    private EventBus eventBus;
    private ResetCommentsCommand resetCommentsCommand;
    private SaveCommentsCommand saveCommentsCommand;

    [SetUp]
    public void SetUp()
    {
        requestBus = new Mock<IRequestBus>();
        eventBus = new EventBus();
        resetCommentsCommand = new ResetCommentsCommand(requestBus.Object, eventBus);
        saveCommentsCommand = new SaveCommentsCommand(requestBus.Object, eventBus);
    }

    [Test]
    public void successfully_instantiated()
    {
        new CommentsViewModel(requestBus.Object, eventBus, resetCommentsCommand, saveCommentsCommand);
    }

    [Test]
    public void throws_if_mediator_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(null, eventBus, resetCommentsCommand, saveCommentsCommand));
    }

    [Test]
    public void throws_if_resetCommentsCommand_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(requestBus.Object, eventBus, null, saveCommentsCommand));
    }

    [Test]
    public void throws_if_saveCommentsCommand_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(requestBus.Object, eventBus, resetCommentsCommand, null));
    }

    [Test]
    public void Constructor_sends_PresentCommentsRequest_to_mediator()
    {
        CommentsViewModel viewModel = new(requestBus.Object, eventBus, resetCommentsCommand, saveCommentsCommand);

        requestBus.Verify(x => x.Send(It.IsAny<PresentCommentsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Constructor_initializes_Comment_with_value_returned_in_the_response()
    {
        PresentCommentsResponse presentCommentsResponse = new()
        {
            Comments = "ha ha ha"
        };
        requestBus
            .Setup(x => x.Send(It.IsAny<PresentCommentsRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(presentCommentsResponse));
        CommentsViewModel viewModel = new(requestBus.Object, eventBus, resetCommentsCommand, saveCommentsCommand);

        Assert.That(viewModel.Comments, Is.EqualTo("ha ha ha"));
    }
}