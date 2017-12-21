// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using DustInTheWind.ActiveTime.Commands;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.Services;

namespace DustInTheWind.ActiveTime.ViewModels
{
    internal class CurrentDateViewModel : ViewModelBase
    {
        private readonly ICurrentDay currentDay;

        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set
            {
                if (date == value)
                    return;

                date = value;
                OnPropertyChanged();

                currentDay.Date = value;
            }
        }

        public CalendarCommand CalendarCommand { get; }
        public DecrementDayCommand DecrementDayCommand { get; }
        public IncrementDayCommand IncrementDayCommand { get; }

        public CurrentDateViewModel(ICurrentDay currentDay, IShellNavigator shellNavigator)
        {
            this.currentDay = currentDay;

            CalendarCommand = new CalendarCommand(shellNavigator);
            DecrementDayCommand = new DecrementDayCommand(currentDay);
            IncrementDayCommand = new IncrementDayCommand(currentDay);

            Date = currentDay.Date;

            currentDay.DateChanged += HandleCurrentDateChanged;
        }

        private void HandleCurrentDateChanged(object sender, EventArgs e)
        {
            Date = currentDay.Date;
        }
    }
}