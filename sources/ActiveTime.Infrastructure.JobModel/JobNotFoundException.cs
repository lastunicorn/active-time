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


namespace DustInTheWind.ActiveTime.Infrastructure.JobEngine;

[Serializable]
internal class JobNotFoundException : Exception
{
    private const string DefaultMessage = "There is no job with the id {0}.";

    public JobNotFoundException(string jobId)
        : base(string.Format(DefaultMessage, jobId))
    {
    }

    public JobNotFoundException(string jobId, Exception innerException)
        : base(string.Format(DefaultMessage, jobId), innerException)
    {
    }
}