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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Recording.Stamp
{
    public class StampUseCase : IRequestHandler<StampRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly Scribe scribe;
        private readonly IStatusInfoService statusInfoService;

        public StampUseCase(IUnitOfWork unitOfWork, Scribe scribe, IStatusInfoService statusInfoService)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribe = scribe ?? throw new ArgumentNullException(nameof(scribe));
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
        }

        public Task<Unit> Handle(StampRequest request, CancellationToken cancellationToken)
        {
            statusInfoService.SetStatus("Updating the current record's time.");
            scribe.Stamp();
            statusInfoService.SetStatus("Current record's time has been updated.");

            unitOfWork.Commit();

            return Task.FromResult(Unit.Value);
        }
    }
}