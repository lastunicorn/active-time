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
public class SubscribeTests
{
    [Test]
    public async Task HavingNoSubscriber_WhenEventIsPublished_ThenNothingHappens()
    {
        EventBus eventBus = new();

        DummyEvent dummyEvent = new();
        await eventBus.Publish(dummyEvent);
    }

    [Test]
    public async Task HavingOneSubscriberForDifferentEvent_WhenEventIsPublished_ThenSubscriberIsNotCalled()
    {
        EventBus eventBus = new();
        bool subscriberWasCalled = false;

        eventBus.Subscribe<AnotherDummyEvent>((e, cancellationToken) =>
        {
            subscriberWasCalled = true;
            return Task.CompletedTask;
        });

        DummyEvent dummyEvent = new();
        await eventBus.Publish(dummyEvent);

        subscriberWasCalled.Should().BeFalse();
    }

    [Test]
    public async Task HavingOneSubscriber_WhenEventIsPublished_ThenSubscriberIsCalled()
    {
        EventBus eventBus = new();
        bool subscriberWasCalled = false;

        eventBus.Subscribe<DummyEvent>((e, cancellationToken) =>
        {
            subscriberWasCalled = true;
            return Task.CompletedTask;
        });

        DummyEvent dummyEvent = new();
        await eventBus.Publish(dummyEvent);

        subscriberWasCalled.Should().BeTrue();
    }

    [Test]
    public async Task HavingTwoSubscribers_WhenEventIsPublished_ThenBothSubscribersAreCalled()
    {
        EventBus eventBus = new();
        bool subscriber1WasCalled = false;
        bool subscriber2WasCalled = false;

        eventBus.Subscribe<DummyEvent>((e, cancellationToken) =>
        {
            subscriber1WasCalled = true;
            return Task.CompletedTask;
        });
        
        eventBus.Subscribe<DummyEvent>((e, cancellationToken) =>
        {
            subscriber2WasCalled = true;
            return Task.CompletedTask;
        });

        DummyEvent dummyEvent = new();
        await eventBus.Publish(dummyEvent);

        subscriber1WasCalled.Should().BeTrue();
        subscriber2WasCalled.Should().BeTrue();
    }
}