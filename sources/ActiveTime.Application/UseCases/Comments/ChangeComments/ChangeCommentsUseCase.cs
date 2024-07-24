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
using DustInTheWind.ActiveTime.Application.UseCases.Comments;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Comments.ChangeComments;

internal class ChangeCommentsUseCase : IRequestHandler<ChangeCommentsRequest>
{
    private readonly CurrentDay currentDay;
    private readonly EventBus eventBus;

    public ChangeCommentsUseCase(CurrentDay currentDay, EventBus eventBus)
    {
        this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task Handle(ChangeCommentsRequest request, CancellationToken cancellationToken)
    {
        SetCommentsOnCurrentDay(request.Comments);
        await RaiseCommentChangedEvent();
        await RaiseCommentStateChangedEvent();
    }

    private void SetCommentsOnCurrentDay(string comments)
    {
        currentDay.Comments = comments;
    }

    private async Task RaiseCommentChangedEvent()
    {
        CommentChangedEvent commentChangedEvent = new()
        {
            NewComments = currentDay.Comments
        };
        await eventBus.Publish(commentChangedEvent);
    }

    private async Task RaiseCommentStateChangedEvent()
    {
        CommentStateChangedEvent commentChangedEvent = new()
        {
            CommentsAreSaved = currentDay.AreCommentsSaved
        };
        await eventBus.Publish(commentChangedEvent);
    }
}