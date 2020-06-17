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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public sealed class OverviewViewModel : ViewModelBase
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private string comments;
        public string Comments
        {
            get { return comments; }
            private set
            {
                comments = value;
                OnPropertyChanged();
            }
        }
        
        private DateTime firstDay;
        public DateTime FirstDay
        {
            get { return firstDay; }
            set
            {
                firstDay = value;
                OnPropertyChanged();
                PopulateComments();
            }
        }

        private DateTime lastDay;
        public DateTime LastDay
        {
            get { return lastDay; }
            set
            {
                lastDay = value;
                OnPropertyChanged();
                PopulateComments();
            }
        }

        public OverviewViewModel(IUnitOfWorkFactory unitOfWorkFactory, ITimeProvider timeProvider)
        {
            if (unitOfWorkFactory == null) throw new ArgumentNullException(nameof(unitOfWorkFactory));
            if (timeProvider == null) throw new ArgumentNullException(nameof(timeProvider));

            this.unitOfWorkFactory = unitOfWorkFactory;

            DateTime today = timeProvider.GetDate();
            firstDay = today.AddDays(-29);
            lastDay = today;

            PopulateComments();
        }

        private void PopulateComments()
        {
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;
                ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;
                
                ReportBuilder reportBuilder = new ReportBuilder(dayCommentRepository, timeRecordRepository)
                {
                    FirstDay = firstDay,
                    LastDay = lastDay
                };
                Comments = reportBuilder.Build();
            }
        }
    }
}
