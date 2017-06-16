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
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.RecorderServiceTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<IScribe> scribMock;
        private Mock<IApplicationService> applicationService;

        [SetUp]
        public void SetUp()
        {
            scribMock = new Mock<IScribe>();
            applicationService = new Mock<IApplicationService>();
        }

        [Test]
        public void throws_if_scrib_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new RecorderService(null, applicationService.Object));
        }

        [Test]
        public void throws_if_applicationService_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new RecorderService(scribMock.Object, null));
        }

        [Test]
        public void successfully_called()
        {
            new RecorderService(scribMock.Object, applicationService.Object);
        }

        [Test]
        public void StampingInterval_initial_value_is_1min()
        {
            RecorderService recorderService = new RecorderService(scribMock.Object, applicationService.Object);

            Assert.That(recorderService.StampingInterval, Is.EqualTo(TimeSpan.FromMinutes(1)));
        }
    }
}
