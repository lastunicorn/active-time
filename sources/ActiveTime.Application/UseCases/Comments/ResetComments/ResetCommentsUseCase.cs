﻿// ActiveTime
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
using DustInTheWind.ActiveTime.Application.StatusManagement;
using DustInTheWind.ActiveTime.Application.StatusManagement.ApplicationStatuses;
using DustInTheWind.ActiveTime.Application.UseCases.Comments;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Comments.ResetComments;

internal sealed class ResetCommentsUseCase : IRequestHandler<ResetCommentsRequest>, IDisposable
{
    private readonly IUnitOfWork unitOfWork;
    private readonly EventBus eventBus;
    private readonly CurrentDay currentDay;
    private readonly StatusInfoService statusInfoService;

    public ResetCommentsUseCase(IUnitOfWork unitOfWork, EventBus eventBus, CurrentDay currentDay, StatusInfoService statusInfoService)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
    }

    public async Task Handle(ResetCommentsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DateTime currentDate = currentDay.Date;

            DateRecord dateRecord = RetrieveDateRecordFromDb(currentDate);
            SetCommentOnCurrentDay(dateRecord?.Comment);
            await RaiseCommentChangedEvent();
            await RaiseCommentStateChangedEvent();
            UpdateApplicationStatus();
        }
        finally
        {
            Dispose();
        }
    }

    private DateRecord RetrieveDateRecordFromDb(DateTime currentDate)
    {
        return unitOfWork.DateRecordRepository.GetByDate(currentDate);
    }

    private void SetCommentOnCurrentDay(string comments)
    {
        currentDay.ResetComments(comments);
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

    private void UpdateApplicationStatus()
    {
        statusInfoService.SetStatus<ResetStatusMessage>();
    }

    public void Dispose()
    {
        unitOfWork?.Dispose();
    }
}