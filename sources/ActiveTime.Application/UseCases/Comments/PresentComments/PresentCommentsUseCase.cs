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
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Comments.PresentComments
{
    internal sealed class PresentCommentsUseCase : IRequestHandler<PresentCommentsRequest, PresentCommentsResponse>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CurrentDay currentDay;

        public PresentCommentsUseCase(IUnitOfWork unitOfWork, CurrentDay currentDay)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        }

        public Task<PresentCommentsResponse> Handle(PresentCommentsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string comments = RetrieveComments();

                PresentCommentsResponse response = new()
                {
                    Comments = comments
                };

                return Task.FromResult(response);
            }
            finally
            {
                Dispose();
            }
        }

        private string RetrieveComments()
        {
            if (currentDay.AreCommentsLoaded)
                return currentDay.Comments;

            DateRecord dateRecord = unitOfWork.DateRecordRepository.GetByDate(currentDay.Date);
            string comments = dateRecord?.Comment;

            currentDay.Comments = comments;

            return comments;
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }
    }
}