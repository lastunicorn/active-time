using System;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Miscellaneous.PresentCurrentDateInfo;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class TimeReportViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        private TimeSpan activeTime;

        public TimeSpan ActiveTime
        {
            get => activeTime;
            private set
            {
                activeTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get => totalTime;
            private set
            {
                totalTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? beginTime;

        public TimeSpan? BeginTime
        {
            get => beginTime;
            set
            {
                beginTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? estimatedEndTime;

        public TimeSpan? EstimatedEndTime
        {
            get => estimatedEndTime;
            set
            {
                estimatedEndTime = value;
                OnPropertyChanged();
            }
        }

        public TimeReportViewModel(IMediator mediator, EventBus eventBus, ILogger logger)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            eventBus.Subscribe(EventNames.Recorder.Started, HandleCurrentDateChanged);
            eventBus.Subscribe(EventNames.Recorder.Stopped, HandleCurrentDateChanged);
            eventBus.Subscribe(EventNames.Recorder.Stamped, HandleCurrentDateChanged);

            _ = Initialize();
        }

        private void HandleCurrentDateChanged(EventParameters parameters)
        {
            _ = Initialize();
        }

        private async Task Initialize()
        {
            try
            {
                PresentCurrentDateInfoRequest request = new PresentCurrentDateInfoRequest();
                PresentCurrentDateInfoResponse response = await mediator.Send(request);

                ActiveTime = response.ActiveTime;
                TotalTime = response.TotalTime;
                BeginTime = response.BeginTime;
                EstimatedEndTime = response.EstimatedEndTime;
            }
            catch (Exception ex)
            {
                logger.Log("ERROR: " + ex);
            }
        }
    }
}