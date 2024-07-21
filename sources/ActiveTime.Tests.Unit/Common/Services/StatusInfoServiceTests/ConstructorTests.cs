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

using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseModel;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Common.Services.StatusInfoServiceTests;

[TestFixture]
public class ConstructorTests
{
    [Test]
    public void successfully_instantiated()
    {
        EventBus eventBus = new();
        new StatusInfoService(eventBus);
    }

    [Test]
    public void DefaultStatusText_constant_value()
    {
        Assert.That(StatusInfoService.DefaultStatusText, Is.EqualTo("Ready"));
    }

    [Test]
    public void StatusText_initial_value()
    {
        EventBus eventBus = new();
        StatusInfoService statusInfoService = new(eventBus);

        Assert.That(statusInfoService.StatusText, Is.EqualTo("Ready"));
    }
}