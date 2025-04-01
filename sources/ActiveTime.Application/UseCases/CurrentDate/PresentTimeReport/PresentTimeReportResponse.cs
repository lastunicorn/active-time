// ActiveTime
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

using System;
using DustInTheWind.ActiveTime.Application.Recording;

namespace DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.PresentTimeReport
{
    public class PresentTimeReportResponse
    {
        public DayTimeInterval[] Records { get; set; }

        public TimeSpan ActiveTime { get; set; }

        public TimeSpan TotalTime { get; set; }

        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? EstimatedEndTime { get; set; }
    }
}