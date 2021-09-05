using System;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentOverview
{
    public class PresentOverviewRequest : IRequest<PresentOverviewResponse>
    {
        public DateTime FirstDay { get; set; }

        public DateTime LastDay { get; set; }
    }
}