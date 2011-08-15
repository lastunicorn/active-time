using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests.DayCommentRepositoryTests
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
