//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using DustInTheWind.ActiveTime.Common.Events;
//using DustInTheWind.ActiveTime.Common.Persistence;
//using DustInTheWind.ActiveTime.Common.Recording;
//using Microsoft.Practices.Prism.Events;

//namespace DustInTheWind.ActiveTime.RecorderModule.Services
//{
//    class RecorderService : IRecorder
//    {
//        private readonly Recorder recorder;
//        private readonly Timer timer;
//        private TimeSpan stampingInterval;

//        public RecorderService(ITimeRecordRepository timeRecordRepository, IEventAggregator eventAggregator)
//        {
//            if (timeRecordRepository == null)
//                throw new ArgumentNullException("timeRecordRepository");

//            if (eventAggregator == null)
//                throw new ArgumentNullException("eventAggregator");

//            recorder = new Recorder(timeRecordRepository);
//            timer = new Timer(timer_tick);
//            stampingInterval = TimeSpan.FromMinutes(1);

//            ApplicationExitEvent applicationExitEvent = eventAggregator.GetEvent<ApplicationExitEvent>();
//            if (applicationExitEvent != null)
//                applicationExitEvent.Subscribe(new Action<object>(OnApplicationExitEvent));
//        }

//        private void timer_tick(object o)
//        {
//            recorder.Stamp();
//        }

//        private void OnApplicationExitEvent(object o)
//        {
//            recorder.Stop();
//        }

//        public RecorderState State
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public event EventHandler Started;

//        public event EventHandler Stopped;

//        public event EventHandler Stamped;

//        public event EventHandler Stamping;

//        public void Start()
//        {
//            throw new NotImplementedException();
//        }

//        public void Stop(bool deleteLastRecord = false)
//        {
//            throw new NotImplementedException();
//        }

//        public TimeSpan? GetTimeFromLastStop()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
