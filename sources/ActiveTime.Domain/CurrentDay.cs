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

namespace DustInTheWind.ActiveTime.Common
{
    public class CurrentDay
    {
        private string comments;
        private DateTime date;

        public int? TimeRecordId { get; set; }

        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                comments = null;

                AreCommentsLoaded = false;
                AreCommentsSaved = true;
            }
        }

        public string Comments
        {
            get => comments;
            set
            {
                comments = value;
                AreCommentsSaved = false;
            }
        }

        public bool AreCommentsLoaded { get; private set; }

        public bool AreCommentsSaved { get; private set; }

        public CurrentDay()
        {
            Date = DateTime.Today;
            AreCommentsSaved = true;
        }

        public void ResetComments(string newComments)
        {
            Comments = newComments;
            AreCommentsLoaded = true;
            AreCommentsSaved = true;
        }

        public void AcceptModifications()
        {
            AreCommentsSaved = true;
        }

        public void IncrementDate()
        {
            Date = Date.AddDays(1);
        }

        public void DecrementDate()
        {
            Date = Date.AddDays(-1);
        }
    }
}