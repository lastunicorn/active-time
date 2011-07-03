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
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.UI.IViews;
using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.Controllers
{
    internal class CommentsPresenter
    {
        private Dal dal;
        private DateTime date;
        private CommentsModel model;
        private ICommentsView view;

        public CommentsPresenter(ICommentsView view, Dal dal, DateTime date)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            if (dal == null)
                throw new ArgumentNullException("dal");

            if (date == null)
                throw new ArgumentNullException("date");

            this.view = view;
            this.dal = dal;
            this.date = date;

            model = new CommentsModel();

            model.Date = date;
            model.DateChanged += new EventHandler(model_DateChanged);
        }

        private void model_DateChanged(object sender, EventArgs e)
        {
            UpdateModel();
        }

        private void UpdateModel()
        {
            model.Comment = dal.GetComment(model.Date);
        }

        public void SaveButtonClicked()
        {
            try
            {
                SaveInternal();
                model.DataIsChanged = false;
                view.CloseWithOk();
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void ApplyButtonClicked()
        {
            try
            {
                SaveInternal();
                model.DataIsChanged = false;
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void SaveInternal()
        {
            try
            {
                dal.UpdateOrInsertComment(model.Date, model.Comment);
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void CancelButtonClicked()
        {
            try
            {
                view.CloseWithCancel();
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }

        public void WindowLoaded()
        {
            try
            {
                model.InitializationMode = true;
                try
                {
                    UpdateModel();
                }
                finally
                {
                    model.InitializationMode = false;
                }
                view.Model = model;
            }
            catch (Exception ex)
            {
                view.DisplayError(ex);
            }
        }
    }
}
