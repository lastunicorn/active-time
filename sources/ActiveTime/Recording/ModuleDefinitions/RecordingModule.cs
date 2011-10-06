using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Main.Services;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.Recording.ModuleDefinitions
{
    public class RecordingModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        public RecordingModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            unityContainer.RegisterInstance<ITimeRecordRepository>(new TimeRecordRepository(), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IRecorder, Recorder>(new ContainerControlledLifetimeManager());

            IRecorder recorder = unityContainer.Resolve<IRecorder>();

            recorder.Started += new EventHandler(recorder_Started);
            recorder.Stopped += new EventHandler(recorder_Stopped);
            recorder.Start();
        }

        public void recorder_Started(object sender, EventArgs e)
        {
            //if (trayIconManager != null)
            {
                //trayIconManager.SetIconOn();
                //trayIconManager.StartEnabled = false;
                //trayIconManager.StopEnabled = true;

                IReminder reminder = unityContainer.Resolve<IReminder>();
                reminder.Start(DustInTheWind.ActiveTime.Properties.Settings.Default.ReminderInterval);
            }
        }

        public void recorder_Stopped(object sender, EventArgs e)
        {
            //if (trayIconManager != null)
            {
                //trayIconManager.SetIconOff();
                //trayIconManager.StartEnabled = true;
                //trayIconManager.StopEnabled = false;

                IReminder reminder = unityContainer.Resolve<IReminder>();
                reminder.Stop();
            }
        }
    }
}
