using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace DustInTheWind.ActiveTime.Persistence
{
    class RecordRepository : RepositoryBase, IRecordRepository
    {
        public void Add(Record record)
        {
        }

        public void Update(Record record)
        {
        }

        public void Delete(Record record)
        {
        }

        public Record GetById(int id)
        {
            throw new NotImplementedException();
        }

        public RecordList GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
