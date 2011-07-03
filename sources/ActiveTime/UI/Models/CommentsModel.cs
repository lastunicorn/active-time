// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using System.ComponentModel;

namespace DustInTheWind.ActiveTime.UI.Models
{
    public class CommentsModel : INotifyPropertyChanged
    {
        private bool initializationMode = false;

        public bool InitializationMode
        {
            get { return initializationMode; }
            set { initializationMode = value; }
        }


        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Date"));
                OnDateChanged(EventArgs.Empty);
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));

                if (!initializationMode)
                {
                    DataIsChanged = true;
                }
            }
        }

        private bool dataIsChanged;
        public bool DataIsChanged
        {
            get { return dataIsChanged; }
            set
            {
                dataIsChanged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataIsChanged"));
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region DateChanged

        public event EventHandler DateChanged;

        protected virtual void OnDateChanged(EventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, e);
        }

        #endregion
    }
}
