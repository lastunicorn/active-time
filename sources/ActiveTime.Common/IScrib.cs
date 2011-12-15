using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Common
{
    public interface IScrib
    {
        void Stamp();
        void StampNew();
        void DeleteDatabaseRecord();
        TimeSpan? GetTimeFromLastStamp();
    }
}
