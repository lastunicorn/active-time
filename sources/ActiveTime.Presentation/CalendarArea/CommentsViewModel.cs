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
using DustInTheWind.ActiveTime.Application.UseCases.Comments.ChangeComments;
using DustInTheWind.ActiveTime.Application.UseCases.Comments.PresentComments;
using DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.DecrementDate;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseModel;
using DustInTheWind.ActiveTime.Presentation.Commands;

namespace DustInTheWind.ActiveTime.Presentation.CalendarArea;

public class CommentsViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;

    private string comments;

    public string Comments
    {
        get => comments;
        set
        {
            if (comments == value)
                return;

            comments = value;
            OnPropertyChanged();

            if (!IsInitializing)
                _ = ChangeComments(value);
        }
    }

    private bool commentTextWrap = true;

    public bool CommentTextWrap
    {
        get => commentTextWrap;
        set
        {
            commentTextWrap = value;
            OnPropertyChanged();
        }
    }

    public ResetCommentsCommand ResetCommentsCommand { get; }

    public SaveCommentsCommand SaveCommentsCommand { get; }

    public CommentsViewModel(IRequestBus requestBus, EventBus eventBus,
        ResetCommentsCommand resetCommentsCommand, SaveCommentsCommand saveCommentsCommand)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));

        ResetCommentsCommand = resetCommentsCommand ?? throw new ArgumentNullException(nameof(resetCommentsCommand));
        SaveCommentsCommand = saveCommentsCommand ?? throw new ArgumentNullException(nameof(saveCommentsCommand));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<CurrentDateChangedEvent>(HandleCurrentDateChanged);
        eventBus.Subscribe<CommentChangedEvent>(HandleCommentChanged);

        _ = Initialize();
    }

    private async Task HandleCurrentDateChanged(CurrentDateChangedEvent ev, CancellationToken cancellationToken)
    {
        if (!IsInitializing)
            await Initialize();
    }

    private Task HandleCommentChanged(CommentChangedEvent ev, CancellationToken cancellationToken)
    {
        RunAsInitialization(() =>
        {
            Comments = ev.NewComments;
        });

        return Task.CompletedTask;
    }

    private async Task Initialize()
    {
        PresentCommentsRequest request = new();
        PresentCommentsResponse response = await requestBus.Send<PresentCommentsRequest, PresentCommentsResponse>(request);

        RunAsInitialization(() =>
        {
            Comments = response.Comments;
        });
    }

    private async Task ChangeComments(string value)
    {
        ChangeCommentsRequest request = new()
        {
            Comments = value
        };

        await requestBus.Send(request);
    }
}