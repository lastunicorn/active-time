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
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.Comments.ChangeComments;
using DustInTheWind.ActiveTime.Application.Comments.PresentComments;
using DustInTheWind.ActiveTime.Presentation.Commands;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        private readonly IMediator mediator;

        private string comments;

        public string Comments
        {
            get => comments;
            set
            {
                comments = value;
                OnPropertyChanged();

                _ = ChangeComments(value);
            }
        }

        private bool commentTextWrap = true;

        public bool CommentTextWrap
        {
            get => commentTextWrap;
            set
            {
                commentTextWrap = value;
                OnPropertyChanged();
            }
        }

        public ResetCommentsCommand ResetCommentsCommand { get; }

        public SaveCommentsCommand SaveCommentsCommand { get; }

        public CommentsViewModel(IMediator mediator, ResetCommentsCommand resetCommentsCommand, SaveCommentsCommand saveCommentsCommand)
        {
            ResetCommentsCommand = resetCommentsCommand ?? throw new ArgumentNullException(nameof(resetCommentsCommand));
            SaveCommentsCommand = saveCommentsCommand ?? throw new ArgumentNullException(nameof(saveCommentsCommand));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _ = Initialize();
        }

        private async Task Initialize()
        {
            PresentCommentsRequest request = new PresentCommentsRequest();
            PresentCommentsResponse response = await mediator.Send(request);

            Comments = response.Comments;
        }

        private async Task ChangeComments(string value)
        {
            ChangeCommentsRequest request = new ChangeCommentsRequest
            {
                Comments = value
            };

            await mediator.Send(request);
        }
    }
}