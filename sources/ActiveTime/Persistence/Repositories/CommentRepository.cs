using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Persistence.Entities;

namespace DustInTheWind.ActiveTime.Persistence
{
    class CommentRepository : RepositoryBase, ICommentRepository
    {
        public void Add(DayComment comment)
        {
            
        }

        public void Update(DayComment comment)
        {
        }

        public void Delete(DayComment comment)
        {
        }

        public DayComment GetById(int id)
        {
            throw new NotImplementedException();
        }

        public DayCommentList GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
