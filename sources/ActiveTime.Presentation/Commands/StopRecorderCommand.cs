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
using DustInTheWind.ActiveTime.Application.UseCases.StopRecording;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.Commands
{
    internal class StopRecorderCommand : CommandBase
    {
        private readonly IRecorderService recorder;
        private readonly IMediator mediator;

        public StopRecorderCommand(IRecorderService recorder, IMediator mediator)
        {
            this.recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            recorder.Started += HandleRecorderStarted;
            recorder.Stopped += HandleRecorderStopped;
        }

        private void HandleRecorderStarted(object sender, EventArgs e)
        {
            OnCanExecuteChanged();
        }

        private void HandleRecorderStopped(object sender, EventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            return recorder.State == RecorderState.Running;
        }

        public override void Execute(object parameter)
        {
            StopRecordingRequest request = new StopRecordingRequest();
            mediator.Send(request);
        }
    }
}