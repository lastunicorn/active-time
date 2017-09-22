// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using System.Data.Common;

namespace DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.Repositories
{
    public class CustomDataReader
    {
        private readonly DbDataReader dataReader;

        public CustomDataReader(DbDataReader dataReader)
        {
            if (dataReader == null) throw new ArgumentNullException(nameof(dataReader));

            this.dataReader = dataReader;
        }

        public int ReadInt32FromCurrentTimeRecord(string fieldName)
        {
            try
            {
                object valueAsObject = dataReader[fieldName];
                string valueAsString = valueAsObject.ToString();

                return int.Parse(valueAsString);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Invalid {0} value for the TimeRecord read from the db.", fieldName);
                throw new PersistenceException(errorMessage, ex);
            }
        }

        public DateTime ReadDateTimeFromCurrentTimeRecord(string fieldName)
        {
            try
            {
                object valueAsObject = dataReader[fieldName];
                string valueAsString = valueAsObject.ToString();

                return DateTime.Parse(valueAsString);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Invalid {0} value for the TimeRecord read from the db.", fieldName);
                throw new PersistenceException(errorMessage, ex);
            }
        }

        public TimeSpan ReadTimeOfDayFromCurrentTimeRecord(string fieldName)
        {
            try
            {
                object valueAsObject = dataReader[fieldName];
                string valueAsString = valueAsObject.ToString();
                DateTime dateTime = DateTime.Parse(valueAsString);

                return dateTime.TimeOfDay;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Invalid {0} value for the TimeRecord read from the db.", fieldName);
                throw new PersistenceException(errorMessage, ex);
            }
        }

        public T ReadEnumFromCurrentTimeRecord<T>(string fieldName)
        {
            try
            {
                object valueAsObject = dataReader[fieldName];
                string valueAsString = valueAsObject.ToString();

                return (T)Enum.Parse(typeof(T), valueAsString);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Invalid {0} value for the TimeRecord read from the db.", fieldName);
                throw new PersistenceException(errorMessage, ex);
            }
        }
    }
}