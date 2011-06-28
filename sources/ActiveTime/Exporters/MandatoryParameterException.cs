using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DustInTheWind.ActiveTime.Exporters
{
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
