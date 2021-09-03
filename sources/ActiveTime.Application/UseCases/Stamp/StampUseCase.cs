using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Stamp
{
    public class StampUseCase : IRequestHandler<StampRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ScribeEx scribeEx;

        public StampUseCase(IUnitOfWork unitOfWork, ScribeEx scribeEx)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribeEx = scribeEx ?? throw new ArgumentNullException(nameof(scribeEx));
        }

        public Task<Unit> Handle(StampRequest request, CancellationToken cancellationToken)
        {
            scribeEx.Stamp();

            unitOfWork.Commit();

            return Task.FromResult(Unit.Value);
        }
    }
}