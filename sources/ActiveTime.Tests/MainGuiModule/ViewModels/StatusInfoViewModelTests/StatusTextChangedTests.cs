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
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.MainGuiModule.ViewModels;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels.StatusInfoViewModelTests
{
    [TestFixture]
    public class StatusTextChangedTests
    {
        private Mock<IStatusInfoService> statusInfoService;
        private StatusInfoViewModel statusInfoViewModel;

        [SetUp]
        public void SetUp()
        {
            statusInfoService = new Mock<IStatusInfoService>();
            statusInfoViewModel = new StatusInfoViewModel(statusInfoService.Object);
        }

        [Test]
        public void StatusText_is_updated_when_service_raises_StatusTextChanged_event()
        {
            const string statusText = "some status text";
            statusInfoService.Setup(x => x.StatusText).Returns(statusText);

            statusInfoService.Raise(x => x.StatusTextChanged += null, EventArgs.Empty);

            Assert.That(statusInfoViewModel.StatusText, Is.EqualTo(statusText));
        }

        [Test]
        public void NotifyPropertyChange_event_is_raised_when_service_raises_StatusTextChanged_event()
        {
            bool eventWasRaised = false;
            statusInfoViewModel.PropertyChanged += (sender, args) => eventWasRaised = true;

            statusInfoService.Raise(x => x.StatusTextChanged += null, EventArgs.Empty);

            Assert.That(eventWasRaised, Is.True);
        }

        [Test]
        public void when_NotifyPropertyChange_event_is_raised_contains_the_name_of_the_property_StatusText()
        {
            string propertyName = null;
            statusInfoViewModel.PropertyChanged += (sender, args) => propertyName = args.PropertyName;

            statusInfoService.Raise(x => x.StatusTextChanged += null, EventArgs.Empty);

            Assert.That(propertyName, Is.EqualTo("StatusText"));
        }
    }
}
