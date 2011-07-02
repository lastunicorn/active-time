using System;
using System.Runtime.Serialization;

namespace DustInTheWind.ActiveTime.Exporters
{
    /// <summary>
    /// Exception thrown when the list of parameters provided to initialize an exporter does not contain a mandatory value.
    /// </summary>
    [Serializable]
    public class MandatoryParameterException : ApplicationException
    {
        private const string MESSAGE = "The parameter '{0}' is absent.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MandatoryParameterException"/> class with a specified error message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that is absent.</param>
        public MandatoryParameterException(string parameterName)
            : base(string.Format(MESSAGE, parameterName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MandatoryParameterException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public MandatoryParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
