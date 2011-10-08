using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace DustInTheWind.ActiveTime.MainModule.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly IApplicationService applicationService;

        private ICommand exportCommand;
        public ICommand ExportCommand
        {
            get { return exportCommand; }
        }

        private ICommand statisticsCommand;
        public ICommand StatisticsCommand
        {
            get { return statisticsCommand; }
        }

        private ICommand exitCommand;
        public ICommand ExitCommand
        {
            get { return exitCommand; }
        }

        private ICommand aboutCommand;
        public ICommand AboutCommand
        {
            get { return aboutCommand; }
        }

        public MainMenuViewModel(IApplicationService applicationService)
        {
            if (applicationService == null)
                throw new ArgumentNullException("applicationService");

            this.applicationService = applicationService;

            exportCommand = new DelegateCommand(OnExportCommandExecuted);
            statisticsCommand = new DelegateCommand(OnStatisticsCommandExecuted);
            exitCommand = new DelegateCommand(OnExitCommandExecuted);
            aboutCommand = new DelegateCommand(OnAboutCommandExecuted);
        }

        private void OnExportCommandExecuted()
        {
        }

        private void OnStatisticsCommandExecuted()
        {
        }

        private void OnExitCommandExecuted()
        {
            applicationService.Exit();
        }

        private void OnAboutCommandExecuted()
        {
            applicationService.ShowAboutWindow();
        }
    }
}
