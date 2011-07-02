
namespace DustInTheWind.ActiveTime.Recording
{
    /// <summary>
    /// Represents the state of a <see cref="Recorder"/> instance.
    /// </summary>
    public enum RecorderState
    {
        /// <summary>
        /// The recorder is stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// The recorder is running and updating the values in the database.
        /// </summary>
        Running
    }
}
