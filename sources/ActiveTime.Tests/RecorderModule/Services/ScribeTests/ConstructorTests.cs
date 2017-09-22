// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Recording.Module.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.ScribeTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<ITimeProvider> timeProvider;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        [SetUp]
        public void SetUp()
        {
            timeProvider = new Mock<ITimeProvider>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
        }

        [Test]
        public void Constructor()
        {
            new Scribe(timeProvider.Object, unitOfWorkFactory.Object);
        }

        [Test]
        public void Constructor_with_null_repository()
        {
            Assert.Throws<ArgumentNullException>(() => new Scribe(timeProvider.Object, null));
        }

        [Test]
        public void Constructor_with_null_timeProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new Scribe(null, unitOfWorkFactory.Object));
        }
    }
}
