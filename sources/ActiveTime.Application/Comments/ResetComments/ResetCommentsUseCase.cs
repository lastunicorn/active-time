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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Comments.ResetComments
{
    internal sealed class ResetCommentsUseCase : IRequestHandler<ResetCommentsRequest>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly EventBus eventBus;
        private readonly CurrentDay currentDay;

        public ResetCommentsUseCase(IUnitOfWork unitOfWork, EventBus eventBus, CurrentDay currentDay)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        }

        public Task<Unit> Handle(ResetCommentsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime currentDate = currentDay.Date;

                DateRecord dateRecord = RetrieveDateRecordFromDb(currentDate);
                SetCommentOnCurrentDay(dateRecord?.Comment);
                RaiseCommentChangedEvent();

                return Task.FromResult(Unit.Value);
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

        private void RaiseCommentChangedEvent()
        {
            eventBus.Raise(EventNames.CurrentDate.CommentChanged);
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }
    }
}