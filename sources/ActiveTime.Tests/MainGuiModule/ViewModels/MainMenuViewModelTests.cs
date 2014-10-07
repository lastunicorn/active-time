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

using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.ViewModels;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels
{
    [TestFixture]
    public class MainMenuViewModelTests
    {
        private MainMenuViewModel mainMenuViewModel;
        private Mock<IApplicationService> applicationService;
        private Mock<IShellNavigator> shellNavigator;
        private Mock<IRecorderService> recorderService;

        [SetUp]
        public void SetUp()
        {
            applicationService = new Mock<IApplicationService>();
            shellNavigator = new Mock<IShellNavigator>();
            recorderService = new Mock<IRecorderService>();

            mainMenuViewModel = new MainMenuViewModel(applicationService.Object, shellNavigator.Object, recorderService.Object);
        }

        [Test]
        public void when_OverviewCommand_invoked_shellNavigator_is_used_to_navigate_to_OverviewShell()
        {
            mainMenuViewModel.OverviewCommand.Execute(null);

            shellNavigator.Verify(x => x.Navigate(ShellNames.OverviewShell, null), Times.Once());
        }

        [Test]
        public void when_ExitCommand_invoked_applicationService_Exit_is_called()
        {
            mainMenuViewModel.ExitCommand.Execute(null);

            applicationService.Verify(x => x.Exit(), Times.Once());
        }

        [Test]
        public void when_StartCommand_invoked_recorder_is_started()
        {
            mainMenuViewModel.StartCommand.Execute(null);

            recorderService.Verify(x => x.Start(), Times.Once());
        }

        [Test]
        public void when_StopCommand_invoked_recorder_is_stoped()
        {
            mainMenuViewModel.StopCommand.Execute(null);

            recorderService.Verify(x => x.Stop(false), Times.Once());
        }
    }
}
