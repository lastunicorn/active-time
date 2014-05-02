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
using DustInTheWind.ActiveTime.PersistenceModule.NHibernate.Repositories;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.NHibernate.Repositories.DayCommentRepositoryTests
{
    [TestFixture]
    public class DayCommentRepositoryCrudTests : RepositoryCrudTestsBase<DayComment, DayCommentRepository>
    {

        protected override DayCommentRepository CreateRepository()
        {
            return new DayCommentRepository(CurrentSession);
        }

        private DateTime date = new DateTime(2000, 06, 13);
        private int index = 0;

        protected override DayComment CreateEntity()
        {
            return new DayComment
            {
                Date = date.AddDays(index++),
                Comment = "some comment"
            };
        }

        protected override void ModifyAllButBusinessKey(DayComment entity)
        {
            entity.Comment = "some other comment";
        }

        protected override void ModifyBusinessKey(DayComment entity)
        {
            entity.Date = new DateTime(2011, 03, 05);
        }

        //protected override void AssertAreEqual(DayComment expectedEntity, DayComment actualEntity)
        //{
        //    Assert.That(actualEntity.Id, Is.EqualTo(expectedEntity.Id));
        //    Assert.That(actualEntity.Date, Is.EqualTo(expectedEntity.Date));
        //    Assert.That(actualEntity.Comment, Is.EqualTo(expectedEntity.Comment));
        //}

        protected override bool HasBusinessKey
        {
            get { return true; }
        }

        protected override bool IsBusinessKeyMutable
        {
            get { return false; }
        }

        protected override ICollection<DayComment> CreateEntities(int count)
        {
            List<DayComment> list = new List<DayComment>();

            for (int i = 0; i < count; i++)
            {
                list.Add(CreateEntity());
            }

            return list;
        }

        protected override bool AreEquals(DayComment a, DayComment b)
        {
            return a != null && b != null && a.Id == b.Id && a.Date == b.Date && a.Comment == b.Comment;
        }
    }
}
