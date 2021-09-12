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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Application.Recording.StartRecording;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.Tray.Commands
{
    public class StartRecorderCommand : ICommand
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;
        
        private JobState recorderState = JobState.Stopped;

        public JobState RecorderState
        {
            get => recorderState;
            set
            {
                recorderState = value;
                OnCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged;

        public StartRecorderCommand(IMediator mediator, ILogger logger, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            eventBus.Subscribe(EventNames.Recorder.Started, HandleRecorderStarted);
            eventBus.Subscribe(EventNames.Recorder.Stopped, HandleRecorderStopped);
        }

        private void HandleRecorderStarted(EventParameters parameters)
        {
            RecorderState = JobState.Running;
        }

        private void HandleRecorderStopped(EventParameters parameters)
        {
            RecorderState = JobState.Stopped;
        }

        public bool CanExecute(object parameter)
        {
            return RecorderState == JobState.Stopped;
        }

        public void Execute(object parameter)
        {
            _ = StartRecording();
        }

        private async Task StartRecording()
        {
            StartRecordingRequest request = new StartRecordingRequest();

            try
            {
                await mediator.Send(request);
            }
            catch (Exception ex)
            {
                logger.Log("ERROR: " + ex);
            }
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}