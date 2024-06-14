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
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Domain.ApplicationStatuses;
using DustInTheWind.ActiveTime.Ports.Persistence;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Recording.Stamp;

public sealed class StampUseCase : IRequestHandler<StampRequest>, IDisposable
{
    private readonly IUnitOfWork unitOfWork;
    private readonly Scribe scribe;
    private readonly StatusInfoService statusInfoService;

    public StampUseCase(IUnitOfWork unitOfWork, Scribe scribe, StatusInfoService statusInfoService)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.scribe = scribe ?? throw new ArgumentNullException(nameof(scribe));
        this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
    }

    public Task Handle(StampRequest request, CancellationToken cancellationToken)
    {
        try
        {
            SetApplicationStatusToStamping();
            scribe.Stamp();
            SetApplicationStatusToStamped();

            unitOfWork.Commit();

            return Task.FromResult(Unit.Value);
        }
        finally
        {
            Dispose();
        }
    }

    private void SetApplicationStatusToStamping()
    {
        statusInfoService.SetStatus<StampingStatusMessage>();
    }

    private void SetApplicationStatusToStamped()
    {
        statusInfoService.SetStatus<StampedStatusMessage>();
    }

    public void Dispose()
    {
        unitOfWork?.Dispose();
    }
}