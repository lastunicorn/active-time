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
using DustInTheWind.ActiveTime.Presentation.Commands;

namespace DustInTheWind.ActiveTime.Presentation.MainArea.ViewModels
{
    public class FrontViewModel : ViewModelBase
    {
        public CurrentDateViewModel CurrentDateViewModel { get; }

        public TimeReportViewModel TimeReportViewModel { get; }

        public CommentsViewModel CommentsViewModel { get; }

        public DayRecordsViewModel DayRecordsViewModel { get; }


        public RefreshCommand RefreshCommand { get; }

        public DeleteCommand DeleteCommand { get; }

        public DecrementDayCommand DecrementDayCommand { get; }

        public IncrementDateCommand IncrementDateCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontViewModel"/> class.
        /// </summary>
        public FrontViewModel(CurrentDateViewModel currentDateViewModel, TimeReportViewModel timeReportViewModel,
            CommentsViewModel commentsViewModel, DayRecordsViewModel dayRecordsViewModel, RefreshCommand refreshCommand,
            DeleteCommand deleteCommand, DecrementDayCommand decrementDayCommand, IncrementDateCommand incrementDateCommand)
        {
            CurrentDateViewModel = currentDateViewModel ?? throw new ArgumentNullException(nameof(currentDateViewModel));
            TimeReportViewModel = timeReportViewModel ?? throw new ArgumentNullException(nameof(timeReportViewModel));
            CommentsViewModel = commentsViewModel ?? throw new ArgumentNullException(nameof(commentsViewModel));
            DayRecordsViewModel = dayRecordsViewModel ?? throw new ArgumentNullException(nameof(dayRecordsViewModel));

            RefreshCommand = refreshCommand ?? throw new ArgumentNullException(nameof(refreshCommand));
            DeleteCommand = deleteCommand ?? throw new ArgumentNullException(nameof(deleteCommand));
            DecrementDayCommand = decrementDayCommand ?? throw new ArgumentNullException(nameof(decrementDayCommand));
            IncrementDateCommand = incrementDateCommand ?? throw new ArgumentNullException(nameof(incrementDateCommand));
        }
    }
}