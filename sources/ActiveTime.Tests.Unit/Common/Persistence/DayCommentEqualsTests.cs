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

using DustInTheWind.ActiveTime.Domain;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Common.Persistence;

[TestFixture]
public class DayCommentEqualsTests
{
    #region Equals

    [Test]
    public void TestEqualsOk()
    {
        DateRecord dateComment1 = BuildNewDayComment();
        DateRecord dateComment2 = BuildNewDayComment();

        bool actualValue = dateComment1.Equals(dateComment2);

        Assert.That(actualValue, Is.True);
    }

    [Test]
    public void TestEquals_DifferentId()
    {
        DateRecord dateComment1 = BuildNewDayComment();
        DateRecord dateComment2 = BuildNewDayComment();
        dateComment2.Id = 10;

        bool actualValue = dateComment1.Equals(dateComment2);

        Assert.That(actualValue, Is.True);
    }

    [Test]
    public void TestEquals_DifferentDate()
    {
        DateRecord dateComment1 = BuildNewDayComment();
        DateRecord dateComment2 = BuildNewDayComment();
        dateComment2.Date = new DateTime(2011, 03, 05);

        bool actualValue = dateComment1.Equals(dateComment2);

        Assert.That(actualValue, Is.False);
    }

    [Test]
    public void TestEquals_DifferentComment()
    {
        DateRecord dateComment1 = BuildNewDayComment();
        DateRecord dateComment2 = BuildNewDayComment();
        dateComment2.Comment = "some different comment";

        bool actualValue = dateComment1.Equals(dateComment2);

        Assert.That(actualValue, Is.True);
    }

    [Test]
    public void TestEquals_AllDifferent()
    {
        DateRecord dateComment1 = BuildNewDayComment();
        DateRecord dateComment2 = BuildNewDayComment();
        dateComment2.Id = 10;
        dateComment2.Date = new DateTime(2011, 03, 05);
        dateComment2.Comment = "some different comment";

        bool actualValue = dateComment1.Equals(dateComment2);

        Assert.That(actualValue, Is.False);
    }

    [Test]
    public void TestEquals_AllDifferentButBusinessKey()
    {
        DateRecord dateComment1 = BuildNewDayComment();
        DateRecord dateComment2 = BuildNewDayComment();
        dateComment2.Id = 10;
        dateComment2.Comment = "some different comment";

        bool actualValue = dateComment1.Equals(dateComment2);

        Assert.That(actualValue, Is.True);
    }

    #endregion

    #region Utils

    private DateRecord BuildNewDayComment()
    {
        return new DateRecord
        {
            Id = 5,
            Date = new DateTime(2000, 06, 13),
            Comment = "some comment"
        };
    }

    #endregion
}