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
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Comments.ResetComments
{
    internal class ResetCommentsUseCase : IRequestHandler<ResetCommentsRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InMemoryState inMemoryState;

        public ResetCommentsUseCase(IUnitOfWork unitOfWork, InMemoryState inMemoryState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.inMemoryState = inMemoryState ?? throw new ArgumentNullException(nameof(inMemoryState));
        }

        public Task Handle(ResetCommentsRequest request, CancellationToken cancellationToken)
        {
            DateTime? currentDate = inMemoryState.CurrentDate;

            if (currentDate == null)
            {
                inMemoryState.Comments = null;
            }
            else
            {
                DateRecord dateRecord = unitOfWork.DateRecordRepository.GetByDate(currentDate.Value);
                inMemoryState.Comments = dateRecord?.Comment;
            }

            return Task.FromResult(Unit.Value);
        }
    }
}