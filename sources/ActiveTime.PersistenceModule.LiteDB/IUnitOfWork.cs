// ActiveTime
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
using DustInTheWind.ActiveTime.Common.Persistence;
using LiteDB;

namespace DustInTheWind.ActiveTime.PersistenceModule.LiteDB
{
    public interface IUnitOfWork : IDisposable
    {
        //LiteDatabase Connection { get; }

        //void ExecuteCommand(Action<DbCommand> action);

        //T ExecuteCommand<T>(Func<DbCommand, T> action);

        void ExecuteAndCommit(Action action);

        //void ExecuteCommandAndCommit(Action<DbCommand> action);

        //T ExecuteCommandAndCommit<T>(Func<DbCommand, T> action);

        void Commit();

        void Rollback();
        ITimeRecordRepository TimeRecordRepository { get; }
    }
}