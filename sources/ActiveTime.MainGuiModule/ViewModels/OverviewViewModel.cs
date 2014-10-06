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
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class OverviewViewModel : ViewModelBase
    {
        private readonly IDayCommentRepository dayCommentRepository;
        private readonly ITimeProvider timeProvider;
        private string comments;

        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                NotifyPropertyChanged("Comments");
            }
        }

        public OverviewViewModel(IDayCommentRepository dayCommentRepository, ITimeProvider timeProvider)
        {
            if (dayCommentRepository == null)
                throw new ArgumentNullException("dayCommentRepository");

            if (timeProvider == null)
                throw new ArgumentNullException("timeProvider");

            this.timeProvider = timeProvider;
            this.dayCommentRepository = dayCommentRepository;

            PopulateComments();
        }

        private void PopulateComments()
        {
            IEnumerable<DayComment> dayComments = RetrieveDayComments();
            Comments = Stringify(dayComments);
        }

        private IEnumerable<DayComment> RetrieveDayComments()
        {
            DateTime lastDate = timeProvider.GetDate();
            DateTime firstDate = lastDate.AddDays(-29);
            return dayCommentRepository.GetByDate(firstDate, lastDate);
        }

        private static string Stringify(IEnumerable<DayComment> comments)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DayComment dayComment in comments)
            {
                sb.Append(dayComment.Date.ToShortDateString());
                sb.Append(" : ");
                sb.Append(dayComment.Comment);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
