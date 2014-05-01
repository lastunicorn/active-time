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
using System.Threading;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.RecorderModule;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.RecorderServiceTests
{
    [TestFixture]
    public class GetTimeFromLastStopTests
    {
        private Mock<IScribe> scribMock;
        private Mock<IApplicationService> applicationService;
        private RecorderService recorderService;

        [SetUp]
        public void SetUp()
        {
            scribMock = new Mock<IScribe>();
            applicationService = new Mock<IApplicationService>();

            recorderService = new RecorderService(scribMock.Object, applicationService.Object);
        }

        [Test]
        public void returns_null_if_not_started()
        {
            RecorderService recorderService = new RecorderService(scribMock.Object, applicationService.Object);

            Assert.That(recorderService.GetTimeFromLastStop(), Is.Null);
        }

        [Test]
        public void returns_null_if_started_but_never_stopped()
        {
            recorderService.Start();

            Assert.That(recorderService.GetTimeFromLastStop(), Is.Null);
        }

        [Test]
        public void returns_correct_value_after_service_is_started_and_stopped()
        {
            recorderService.Start();
            Thread.Sleep(50);
            recorderService.Stop();
            Thread.Sleep(100);

            Assert.That(recorderService.GetTimeFromLastStop(), Is.EqualTo(TimeSpan.FromMilliseconds(100)).Within(TimeSpan.FromMilliseconds(20)));
        }

        [Test]
        public void returns_correct_value_after_service_started_second_time()
        {
            recorderService.Start();
            Thread.Sleep(50);
            recorderService.Stop();
            Thread.Sleep(50);
            recorderService.Start();
            Thread.Sleep(50);

            Assert.That(recorderService.GetTimeFromLastStop(), Is.EqualTo(TimeSpan.FromMilliseconds(100)).Within(TimeSpan.FromMilliseconds(20)));
        }
    }
}
