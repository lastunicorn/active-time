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

using System;
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Domain.Recording.ScribeTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<ISystemClock> systemClock;
        private Mock<IUnitOfWork> unitOfWork;
        private CurrentDay currentDay;

        [SetUp]
        public void SetUp()
        {
            systemClock = new Mock<ISystemClock>();
            unitOfWork = new Mock<IUnitOfWork>();
            currentDay = new CurrentDay();
        }

        [Test]
        public void Constructor()
        {
            new Scribe(systemClock.Object, unitOfWork.Object, currentDay);
        }

        [Test]
        public void Constructor_with_null_unitOfWork()
        {
            Assert.Throws<ArgumentNullException>(() => new Scribe(systemClock.Object, null, currentDay));
        }

        [Test]
        public void Constructor_with_null_timeProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new Scribe(null, unitOfWork.Object, currentDay));
        }

        [Test]
        public void Constructor_with_null_inMemoryState()
        {
            Assert.Throws<ArgumentNullException>(() => new Scribe(systemClock.Object, unitOfWork.Object, null));
        }
    }
}
