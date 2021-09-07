using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.PresentCurrentDateInfo
{
    public class PresentCurrentDateInfoUseCase : IRequestHandler<PresentCurrentDateInfoRequest, PresentCurrentDateInfoResponse>
    {
        public Task<PresentCurrentDateInfoResponse> Handle(PresentCurrentDateInfoRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}