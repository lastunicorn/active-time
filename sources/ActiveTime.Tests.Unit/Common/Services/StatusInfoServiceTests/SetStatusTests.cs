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

using System.Threading;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Domain.ApplicationStatuses;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Common.Services.StatusInfoServiceTests
{
    [TestFixture]
    public class SetStatusTests
    {
        private StatusInfoService statusInfoService;
        private Mock<StatusMessage> applicationStatus;
        private const string Text = "same test text";

        [SetUp]
        public void SetUp()
        {
            EventBus eventBus = new();
            statusInfoService = new StatusInfoService(eventBus);
            applicationStatus = new Mock<StatusMessage>();
        }

        [Test]
        public void HavingAnInstance_WhenSetStatusToSpecificText_ThenStatusTextContainsThatText()
        {
            applicationStatus
                .Setup(x => x.Text)
                .Returns("this is the text");
            statusInfoService.SetStatus(applicationStatus.Object);

            Assert.That(statusInfoService.StatusText, Is.EqualTo("this is the text"));
        }

        [Test]
        public void status_is_reset_after_specified_time()
        {
            statusInfoService.SetStatus(Text, 100);

            Thread.Sleep(100 + TestConstants.TimerDelayAccepted);

            Assert.That(statusInfoService.StatusText, Is.EqualTo(StatusInfoService.DefaultStatusText));
        }

        [Test]
        public void raises_StatusTextChanged_event()
        {
            bool eventRaised = false;
            statusInfoService.StatusTextChanged += (s, e) => eventRaised = true;

            statusInfoService.SetStatus(Text);

            if (!eventRaised)
                Assert.Fail("Event StatusTextChanged was not raised.");
        }
    }
}