using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DustInTheWind.ActiveTime.Exporters
{
    /// <summary>
    /// Exception raised by AutomaticOrderSystemException.
    /// </summary>
    [Serializable]
    public class ExporterException : ApplicationException
    {
        private const string MESSAGE = "Internal error in an exporter plug-in.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterException"/> class.
        /// </summary>
        public ExporterException()
            : base(MESSAGE)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ExporterException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterException"/> class with a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ExporterException(Exception innerException)
            : base(MESSAGE, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ExporterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public ExporterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
