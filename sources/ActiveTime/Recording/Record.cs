using System;

namespace DustInTheWind.ActiveTime.Recording
{
    /// <summary>
    /// Represents an interval of Time within a day.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// The Date for which this record is created.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Gets the Date for which this record is created.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }

        /// <summary>
        /// The Time of day representing the start Time.
        /// </summary>
        private TimeSpan startTime;

        /// <summary>
        /// Gets the Time of day representing the start Time.
        /// </summary>
        public TimeSpan StartTime
        {
            get { return startTime; }
        }

        /// <summary>
        /// The Time of day representing the end Time.
        /// </summary>
        private TimeSpan endTime;

        /// <summary>
        /// Gets the Time of day representing the end Time.
        /// </summary>
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        /// <param name="Date">The Date for which this record is created.</param>
        /// <param name="startTime">The Time of day representing the start Time.</param>
        public Record(DateTime date, TimeSpan startTime)
            : this(date, startTime, startTime)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Record"/> class.
        /// </summary>
        /// <param name="Date">The Date for which this record is created.</param>
        /// <param name="startTime">The Time of day representing the start Time.</param>
        /// <param name="endTime">The Time of day representing the end Time.</param>
        public Record(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            this.date = date;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        #endregion
    }
}
