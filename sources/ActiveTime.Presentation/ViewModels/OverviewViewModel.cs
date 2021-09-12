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
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Miscellaneous.PresentOverview;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.System;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public sealed class OverviewViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        private string comments;

        public string Comments
        {
            get => comments;
            private set
            {
                comments = value;
                OnPropertyChanged();
            }
        }

        private DateTime firstDay;

        public DateTime FirstDay
        {
            get => firstDay;
            set
            {
                firstDay = value;
                OnPropertyChanged();

                _ = PopulateComments();
            }
        }

        private DateTime lastDay;

        public DateTime LastDay
        {
            get => lastDay;
            set
            {
                lastDay = value;
                OnPropertyChanged();

                _ = PopulateComments();
            }
        }

        public OverviewViewModel(ISystemClock systemClock, IMediator mediator, ILogger logger)
        {
            if (systemClock == null) throw new ArgumentNullException(nameof(systemClock));

            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            DateTime today = systemClock.GetCurrentDate();
            firstDay = today.AddDays(-29);
            lastDay = today;

            _ = PopulateComments();
        }

        private async Task PopulateComments()
        {
            Comments = "Loading...";

            try
            {
                PresentOverviewRequest request = new PresentOverviewRequest();
                PresentOverviewResponse response = await mediator.Send(request);

                ReportBuilder reportBuilder = new ReportBuilder(response.Report);
                Comments = reportBuilder.Build();
            }
            catch (Exception ex)
            {
                logger.Log("ERROR: " + ex);
            }
        }
    }
}