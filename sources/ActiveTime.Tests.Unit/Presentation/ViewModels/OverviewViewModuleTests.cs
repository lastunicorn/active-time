// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Presentation.OverviewArea;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Presentation.ViewModels;

[TestFixture]
public class OverviewViewModuleTests
{
    private Mock<IRequestBus> requestBus;
    private Mock<ILogger> logger;

    [SetUp]
    public void SetUp()
    {
        requestBus = new Mock<IRequestBus>();
        logger = new Mock<ILogger>();
    }

    [Test]
    public void WhenOverviewViewModelIsInstantiated_ThenPresentOverviewRequestIsSentToMediator()
    {
        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);

        requestBus.Verify(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void WhenOverviewViewModelIsInstantiated_ThenFirstDayIsInitializedFromTheResponse()
    {
        PresentOverviewResponse response = new()
        {
            FirstDay = new DateTime(1980, 05, 15),
            LastDay = new DateTime(1980, 06, 13)
        };

        requestBus
            .Setup(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(response));

        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);
        Thread.Sleep(50);

        overviewViewModel.FirstDay.Should().Be(new DateTime(1980, 05, 15));
    }

    [Test]
    public void WhenOverviewViewModelIsInstantiated_ThenLastDayIsInitializedFromTheResponse()
    {
        PresentOverviewResponse response = new()
        {
            FirstDay = new DateTime(1980, 05, 15),
            LastDay = new DateTime(1980, 06, 13)
        };

        requestBus
            .Setup(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(response));

        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);
        Thread.Sleep(50);

        overviewViewModel.LastDay.Should().Be(new DateTime(1980, 06, 13));
    }

    [Test]
    public void WhenOverviewViewModelIsInstantiated_ThenCommentsIsNotNull()
    {
        PresentOverviewResponse response = new()
        {
            FirstDay = new DateTime(1980, 05, 15),
            LastDay = new DateTime(1980, 06, 13),
            DayRecords = new List<DateRecord>()
        };

        requestBus
            .Setup(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(response));

        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);

        overviewViewModel.Comments.Should().NotBeNull();
    }

    [Test]
    public void HavingOverviewViewModelInstance_WhenFirsDayIsSet_ThenPresentOverviewRequestIsSentToMediator()
    {
        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);
        requestBus.Invocations.Clear();

        overviewViewModel.FirstDay = new DateTime(2000, 06, 13);

        requestBus.Verify(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void HavingOverviewViewModelInstance_WhenLastDayIsSet_ThenPresentOverviewRequestIsSentToMediator()
    {
        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);
        requestBus.Invocations.Clear();

        overviewViewModel.LastDay = new DateTime(2000, 06, 13);

        requestBus.Verify(x => x.Send(It.IsAny<PresentOverviewRequest>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void HavingOverviewViewModelInstance_WhenSettingFirstDay_ThenPropertyChangedEventIsRaised()
    {
        bool eventWasRaised = false;

        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);

        overviewViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(OverviewViewModel.FirstDay))
                eventWasRaised = true;
        };

        overviewViewModel.FirstDay = new DateTime(2000, 06, 13);

        eventWasRaised.Should().BeTrue();
    }

    [Test]
    public void HavingOverviewViewModelInstance_WhenSettingLastDay_ThenPropertyChangedEventIsRaised()
    {
        bool eventWasRaised = false;

        OverviewViewModel overviewViewModel = new(requestBus.Object, logger.Object);

        overviewViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(OverviewViewModel.LastDay))
                eventWasRaised = true;
        };

        overviewViewModel.LastDay = new DateTime(2000, 06, 13);

        eventWasRaised.Should().BeTrue();
    }
}