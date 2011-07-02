using System;
using System.Runtime.Serialization;

namespace DustInTheWind.ActiveTime.Exporters
{
    /// <summary>
    /// Exception thrown when the list of parameters provided to initialize an exporter
    /// contains a null value for a mandatory parameter.
    /// </summary>
    [Serializable]
    public class ParameterNullException : ApplicationException
    {
        private const string MESSAGE = "The parameter '{0}' is null. A value of type '{1}' is required.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterNullException"/> class with a specified error message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter whose Value is not of the right type.</param>
        public ParameterNullException(string parameterName, Type requestedType)
            : base(string.Format(MESSAGE, parameterName, requestedType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterNullException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public ParameterNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
