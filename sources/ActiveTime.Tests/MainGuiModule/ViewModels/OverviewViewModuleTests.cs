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
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.MainGuiModule.ViewModels;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.MainGuiModule.ViewModels
{
    [TestFixture]
    public class OverviewViewModuleTests
    {
        private Mock<IDayCommentRepository> dayCommentRepository;
        private Mock<ITimeProvider> timeProvider;

        [SetUp]
        public void SetUp()
        {
            dayCommentRepository = new Mock<IDayCommentRepository>();
            dayCommentRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<DayComment>());

            timeProvider = new Mock<ITimeProvider>();
            timeProvider
                .Setup(x => x.GetDate())
                .Returns(new DateTime(1980, 06, 13));
        }

        [Test]
        public void calls_GetByDate_for_the_last_30_days()
        {
            new OverviewViewModel(dayCommentRepository.Object, timeProvider.Object);

            DateTime lastDate = new DateTime(1980, 06, 13);
            DateTime firstDate = new DateTime(1980, 05, 15);
            dayCommentRepository.Verify(x => x.GetByDate(firstDate, lastDate), Times.Once());
        }

        [Test]
        public void populates_Comments_property()
        {
            OverviewViewModel overviewViewModel = new OverviewViewModel(dayCommentRepository.Object, timeProvider.Object);

            Assert.IsNotNull(overviewViewModel.Comments);
        }
    }
}
