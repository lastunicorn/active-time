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
using DustInTheWind.ActiveTime.Domain.ApplicationStatuses;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.ResetStatus;

internal class ResetStatusUseCase : IRequestHandler<ResetStatusRequest>
{
    private readonly StatusInfoService statusInfoService;

    public ResetStatusUseCase(StatusInfoService statusInfoService)
    {
        this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
    }

    public Task Handle(ResetStatusRequest request, CancellationToken cancellationToken)
    {
        statusInfoService.SetStatus<ReadyStatusMessage>();

        return Task.FromResult(Unit.Value);
    }
}