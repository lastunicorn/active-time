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
using DustInTheWind.ActiveTime.Services;

namespace DustInTheWind.ActiveTime.Commands
{
    internal class ResetCommentCommand : CommandBase
    {
        private readonly ICurrentDay currentDay;

        public ResetCommentCommand(ICurrentDay currentDay)
        {
            if (currentDay == null) throw new ArgumentNullException(nameof(currentDay));

            this.currentDay = currentDay;

            currentDay.CommentChanged += HandleCurrentDayCommentChanged;
        }

        private void HandleCurrentDayCommentChanged(object sender, EventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            return !currentDay.IsCommentSaved;
        }

        public override void Execute(object parameter)
        {
            currentDay.ReloadComments();
        }
    }
}
