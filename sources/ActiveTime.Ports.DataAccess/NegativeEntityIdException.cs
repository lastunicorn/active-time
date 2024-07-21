// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

using System.Runtime.Serialization;

namespace DustInTheWind.ActiveTime.Ports.DataAccess;

[Serializable]
public class NegativeEntityIdException : PersistenceException
{
    private const string MessageTemplate = "The id of the entity must be a positive integer. Id = {0}";

    public NegativeEntityIdException(int id)
        : base(BuildMessage(id))
    {
    }

    public NegativeEntityIdException(int id, Exception innerException)
        : base(BuildMessage(id), innerException)
    {
    }

    public NegativeEntityIdException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    private static string BuildMessage(int id)
    {
        return string.Format(MessageTemplate, id);
    }
}