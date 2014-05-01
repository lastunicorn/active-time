﻿// ActiveTime
// Copyright (C) 2011 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
        private const string TemplateMessage = "The parameter '{0}' is absent.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MandatoryParameterException"/> class with a specified error message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that is absent.</param>
        public MandatoryParameterException(string parameterName)
            : base(string.Format(TemplateMessage, parameterName))
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
