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
using DustInTheWind.ActiveTime.Application.Comments;
using DustInTheWind.ActiveTime.Application.Comments.SaveComments;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;

namespace DustInTheWind.ActiveTime.Presentation.Commands;

public class SaveCommentsCommand : CommandBase
{
    private readonly IRequestBus requestBus;
    private bool canExecute;

    public SaveCommentsCommand(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<CommentStateChangedEvent>(HandleCommentStateChanged);
    }

    private Task HandleCommentStateChanged(CommentStateChangedEvent ev, CancellationToken cancellationToken)
    {
        canExecute = !ev.CommentsAreSaved;
        OnCanExecuteChanged();

        return Task.CompletedTask;
    }

    public override bool CanExecute(object parameter)
    {
        return canExecute;
    }

    public override void Execute(object parameter)
    {
        _ = SaveComments();
    }

    private async Task SaveComments()
    {
        SaveCommentsRequest request = new();
        await requestBus.Send(request);
    }
}