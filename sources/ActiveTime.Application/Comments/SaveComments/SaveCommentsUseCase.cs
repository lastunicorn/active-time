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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Ports.Persistence;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Comments.SaveComments;

internal sealed class SaveCommentsUseCase : IRequestHandler<SaveCommentsRequest>, IDisposable
{
    private readonly IUnitOfWork unitOfWork;
    private readonly CurrentDay currentDay;

    public SaveCommentsUseCase(IUnitOfWork unitOfWork, CurrentDay currentDay)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
    }

    public Task Handle(SaveCommentsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DateTime currentDate = currentDay.Date;
            DateRecord dateRecord = GetOrCreateDateRecordFromDb(currentDate);
            dateRecord.Comment = currentDay.Comments;
            unitOfWork.DateRecordRepository.Update(dateRecord);

            unitOfWork.Commit();

            currentDay.AcceptModifications();

            return Task.FromResult(Unit.Value);
        }
        finally
        {
            Dispose();
        }
    }

    private DateRecord GetOrCreateDateRecordFromDb(DateTime date)
    {
        DateRecord dateRecord = unitOfWork.DateRecordRepository.GetByDate(date);

        if (dateRecord == null)
        {
            dateRecord = new DateRecord
            {
                Date = date
            };
            unitOfWork.DateRecordRepository.Add(dateRecord);
        }

        return dateRecord;
    }

    public void Dispose()
    {
        unitOfWork?.Dispose();
    }
}