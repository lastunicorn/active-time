using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DustInTheWind.ActiveTime.Exporters
{
    [Serializable]
    public class ParameterTypeException : ApplicationException
    {
        private const string MESSAGE = "The parameter '{0}' is required to be of type '{1}'. Type '{2}' was provided.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterTypeException"/> class with a specified error message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter whose Value is not of the right type.</param>
        public ParameterTypeException(string parameterName, Type requestedType, Type actualType)
            : base(string.Format(MESSAGE, parameterName, requestedType, actualType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterTypeException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public ParameterTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
