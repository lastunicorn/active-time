using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests
{
    [TestFixture]
    public class CommentRepositoryTests : RepositoryTestsBase
    {
        [Test]
        public void TestAdd_FillId()
        {
            CommentRepository commentRepository = new CommentRepository();

            DayComment dayComment = new DayComment
            {
                Date = new DateTime(2000, 06, 13),
                Comment = "some comment"
            };

            commentRepository.Add(dayComment);

            Assert.That(dayComment.Id, Is.GreaterThan(0));
        }
        [Test]
        public void TestAdd()
        {
            CommentRepository commentRepository = new CommentRepository();
            
            DayComment dayComment = new DayComment
            {
                Date = new DateTime(2000, 06, 13),
                Comment = "some comment"
            };

            commentRepository.Add(dayComment);

            AssertRecordExistsInDb("comments", "id", dayComment.Id.ToString());
        }
    }
}
