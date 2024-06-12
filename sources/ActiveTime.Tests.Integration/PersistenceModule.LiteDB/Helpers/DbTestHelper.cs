﻿// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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

using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Persistence.LiteDB;
using DustInTheWind.ActiveTime.Persistence.LiteDB.Repositories;
using LiteDB;

namespace DustInTheWind.ActiveTime.Tests.Integration.PersistenceModule.LiteDB.Helpers
{
    internal class DbTestHelper
    {
        public const string ConnectionString = Constants.DatabaseFileName;

        public static void ClearDatabase()
        {
            using (LiteDatabase database = new LiteDatabase(ConnectionString))
            {
                ILiteCollection<TimeRecord> timeRecordCollection = database.GetCollection<TimeRecord>(TimeRecordRepository.CollectionName);
                timeRecordCollection.DeleteAll();

                ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(DateRecordRepository.CollectionName);
                dayCommentCollection.DeleteAll();
            }
        }
    }
}