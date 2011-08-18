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

using DustInTheWind.ActiveTime.Persistence.Repositories;
using DustInTheWind.ActiveTime.Recording;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class RecorderDisposeTests
    {
        private Recorder recorder;
        private Mock<ITimeRecordRepository> recordRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            recordRepositoryMock = new Mock<ITimeRecordRepository>();
            recorder = new Recorder(recordRepositoryMock.Object);
        }

        [Test]
        public void TestDisposeTwice()
        {
        }
    }
}
