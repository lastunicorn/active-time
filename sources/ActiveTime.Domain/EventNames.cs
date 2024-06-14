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

namespace DustInTheWind.ActiveTime.Domain
{
    public static class EventNames
    {
        public static class Recorder
        {
            public const string Started = "Recorder.Started";
            public const string Stopped = "Recorder.Stopped";
            public const string Stamping = "Recorder.Stamping";
            public const string Stamped = "Recorder.Stamped";
        }

        public static class Reminder
        {
            public const string Tick = "Reminder.Tick";
        }

        public static class Application
        {
            public const string StatusChanged = "Application.StatusChanged";
        }

        public static class CurrentDate
        {
            public const string CurrentDateChanged = "Application.CurrentDate.CurrentDateChanged";
            public const string CommentChanged = "Application.CurrentDate.CommentChanged";
        }
    }
}