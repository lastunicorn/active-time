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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Miscellaneous.PresentOverview;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Presentation.ViewModels;
using MediatR;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Presentation.ViewModels
{
    [TestFixture]
    public class OverviewViewModuleTests
    {
        private Mock<IMediator> mediator;
        private Mock<ILogger> logger;

        [SetUp]
        public void SetUp()
        {
            mediator = new Mock<IMediator>();
            logger = new Mock<ILogger>();
        }

        [Test]
        public void WhenOverviewViewModelIsInstantiated_ThenPresentOverviewRequestIsSentToMediator()
        {
            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);

            mediator.Verify(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void WhenOverviewViewModelIsInstantiated_ThenFirstDayIsInitializedFromTheResponse()
        {
            PresentOverviewResponse response = new PresentOverviewResponse
            {
                FirstDay = new DateTime(1980, 05, 15),
                LastDay = new DateTime(1980, 06, 13)
            };

            mediator
                .Setup(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);
            Thread.Sleep(50);

            Assert.AreEqual(new DateTime(1980, 05, 15), overviewViewModel.FirstDay);
        }

        [Test]
        public void WhenOverviewViewModelIsInstantiated_ThenLastDayIsInitializedFromTheResponse()
        {
            PresentOverviewResponse response = new PresentOverviewResponse
            {
                FirstDay = new DateTime(1980, 05, 15),
                LastDay = new DateTime(1980, 06, 13)
            };

            mediator
                .Setup(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);
            Thread.Sleep(50);

            Assert.AreEqual(new DateTime(1980, 06, 13), overviewViewModel.LastDay);
        }

        [Test]
        public void WhenOverviewViewModelIsInstantiated_ThenCommentsIsNotNull()
        {
            PresentOverviewResponse response = new PresentOverviewResponse
            {
                FirstDay = new DateTime(1980, 05, 15),
                LastDay = new DateTime(1980, 06, 13),
                DayRecords = new List<DateRecord>()
            };

            mediator
                .Setup(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);

            Assert.IsNotNull(overviewViewModel.Comments);
        }

        [Test]
        public void HavingOverviewViewModelInstance_WhenFirsDayIsSet_ThenPresentOverviewRequestIsSentToMediator()
        {
            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);
            mediator.Invocations.Clear();

            overviewViewModel.FirstDay = new DateTime(2000, 06, 13);

            mediator.Verify(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void HavingOverviewViewModelInstance_WhenLastDayIsSet_ThenPresentOverviewRequestIsSentToMediator()
        {
            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);
            mediator.Invocations.Clear();

            overviewViewModel.LastDay = new DateTime(2000, 06, 13);

            mediator.Verify(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public void HavingOverviewViewModelInstance_WhenSettingFirstDay_ThenPropertyChangedEventIsRaised()
        {
            bool eventWasRaised = false;

            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);

            overviewViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(OverviewViewModel.FirstDay))
                    eventWasRaised = true;
            };

            overviewViewModel.FirstDay = new DateTime(2000, 06, 13);

            Assert.IsTrue(eventWasRaised);
        }

        [Test]
        public void HavingOverviewViewModelInstance_WhenSettingLastDay_ThenPropertyChangedEventIsRaised()
        {
            bool eventWasRaised = false;

            OverviewViewModel overviewViewModel = new OverviewViewModel(mediator.Object, logger.Object);

            overviewViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(OverviewViewModel.LastDay))
                    eventWasRaised = true;
            };

            overviewViewModel.LastDay = new DateTime(2000, 06, 13);

            Assert.IsTrue(eventWasRaised);
        }
    }
}