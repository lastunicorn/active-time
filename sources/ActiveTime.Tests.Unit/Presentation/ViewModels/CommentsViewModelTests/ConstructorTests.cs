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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Comments.PresentComments;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Presentation.Commands;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Presentation.ViewModels.CommentsViewModelTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<IMediator> mediator;
        private EventBus eventBus;
        private ResetCommentsCommand resetCommentsCommand;
        private SaveCommentsCommand saveCommentsCommand;

        [SetUp]
        public void SetUp()
        {
            mediator = new Mock<IMediator>();
            eventBus = new EventBus();
            resetCommentsCommand = new ResetCommentsCommand(mediator.Object, eventBus);
            saveCommentsCommand = new SaveCommentsCommand(mediator.Object, eventBus);
        }

        [Test]
        public void successfully_instantiated()
        {
            new CommentsViewModel(mediator.Object, resetCommentsCommand, saveCommentsCommand);
        }

        [Test]
        public void throws_if_mediator_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(null, resetCommentsCommand, saveCommentsCommand));
        }

        [Test]
        public void throws_if_resetCommentsCommand_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(mediator.Object, null, saveCommentsCommand));
        }

        [Test]
        public void throws_if_saveCommentsCommand_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentsViewModel(mediator.Object, resetCommentsCommand, null));
        }

        [Test]
        public void Constructor_sends_PresentCommentsRequest_to_mediator()
        {
            CommentsViewModel viewModel = new CommentsViewModel(mediator.Object, resetCommentsCommand, saveCommentsCommand);

            mediator.Verify(x => x.Send(It.IsAny<PresentCommentsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Constructor_initializes_Comment_with_value_returned_in_the_response()
        {
            PresentCommentsResponse presentCommentsResponse = new PresentCommentsResponse
            {
                Comments = "ha ha ha"
            };
            mediator
                .Setup(x => x.Send(It.IsAny<PresentCommentsRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(presentCommentsResponse));
            CommentsViewModel viewModel = new CommentsViewModel(mediator.Object, resetCommentsCommand, saveCommentsCommand);

            Assert.That(viewModel.Comments, Is.EqualTo("ha ha ha"));
        }
    }
}