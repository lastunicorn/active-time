namespace DustInTheWind.ActiveTime.Common
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
    }
}