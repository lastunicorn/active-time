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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.ScribeTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<ITimeRecordRepository> repositoryMock;
        private Mock<ITimeProvider> timeProviderMock;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<ITimeRecordRepository>();
            timeProviderMock = new Mock<ITimeProvider>();
        }

        [Test]
        public void Constructor()
        {
            new Scribe(repositoryMock.Object, timeProviderMock.Object);
        }

        [Test]
        public void Constructor_with_null_repository()
        {
            Assert.Throws<ArgumentNullException>(() => new Scribe(null, timeProviderMock.Object));
        }

        [Test]
        public void Constructor_with_null_timeProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new Scribe(repositoryMock.Object, null));
        }
    }
}
