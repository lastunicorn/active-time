using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Common
{
    public interface ITimeProvider
    {
        DateTime GetDateTime();
    }
}
