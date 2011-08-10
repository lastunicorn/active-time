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
        public void Test()
        {
            CommentRepository commentRepository = new CommentRepository();
            
            DayComment dayComment = new DayComment
            {
                Id = 5,
                Date = new DateTime(2000, 06, 13),
                Comment = "some comment"
            };

            commentRepository.Add(dayComment);

            // Check if it is in the database.
            // ...
        }
    }
}
