using System;

namespace DustInTheWind.ActiveTime.Common.ApplicationStatuses
{
    public abstract class ApplicationStatus
    {
        public abstract string Text { get; }

        public override string ToString()
        {
            return Text;
        }

        public static T Create<T>()
            where T : ApplicationStatus
        {
            return Activator.CreateInstance<T>();
        }
    }
}