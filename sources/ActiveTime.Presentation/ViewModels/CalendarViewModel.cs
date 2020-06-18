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
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private readonly CurrentDay currentDay;

        private DateTime? date;

        public DateTime? Date
        {
            get => date;
            set
            {
                if (date == value)
                    return;

                date = value;
                OnPropertyChanged();

                currentDay.Date = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarViewModel"/> class.
        /// </summary>
        public CalendarViewModel(CurrentDay currentDay)
        {
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            
            Date = currentDay.Date;

            currentDay.DateChanged += HandleCurrentDateChanged;
        }

        private void HandleCurrentDateChanged(object sender, EventArgs e)
        {
            Date = currentDay.Date;
        }
    }
}