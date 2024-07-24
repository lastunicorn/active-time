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

using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using FluentAssertions;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Infrastructure.EventBusTests;

[TestFixture]
public class UnsubscribeTests
{
    [Test]
    public async Task HavingSubscriberUnsubscribed_WhenEventIsPublished_ThenSubscriberIsNotCalled()
    {
        EventBus eventBus = new();
        bool subscriberWasCalled = false;

        Task HandleDummyEvent(DummyEvent e, CancellationToken cancellationToken)
        {
            subscriberWasCalled = true;
            return Task.CompletedTask;
        }

        eventBus.Subscribe<DummyEvent>(HandleDummyEvent);
        eventBus.Unsubscribe<DummyEvent>(HandleDummyEvent);

        DummyEvent dummyEvent = new();
        await eventBus.Publish(dummyEvent);

        subscriberWasCalled.Should().BeFalse();
    }

    [Test]
    public void HavingFunctionNotSubscribed_WhenUnsubscribingThatFunction_ThenDoesNotCrush()
    {
        EventBus eventBus = new();

        Task HandleDummyEvent(DummyEvent e, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        eventBus.Unsubscribe<DummyEvent>(HandleDummyEvent);
    }

    [Test]
    public async Task HavingFunctionNotSubscribed_WhenUnsubscribingThatFunctionAndPublishingEvent_ThenFunctionIsNotCalled()
    {
        EventBus eventBus = new();
        bool subscriberWasCalled = false;

        Task HandleDummyEvent(DummyEvent e, CancellationToken cancellationToken)
        {
            subscriberWasCalled = true;
            return Task.CompletedTask;
        }

        eventBus.Unsubscribe<DummyEvent>(HandleDummyEvent);

        DummyEvent dummyEvent = new();
        await eventBus.Publish(dummyEvent);

        subscriberWasCalled.Should().BeFalse();
    }
}