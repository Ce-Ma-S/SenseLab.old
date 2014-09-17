using System;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Allows recording of changes or current states.
    /// </summary>
    public interface IRecorder
    {
        /// <summary>
        /// Whether recording is started by <see cref="Start"/> and not stopped by <see cref="Stop"/>.
        /// </summary>
        bool IsStarted { get; }
        /// <summary>
        /// Whether recording started by <see cref="Start"/> is paused.
        /// </summary>
        bool IsPaused { get; set; }

        /// <summary>
        /// Recordable recorded by this recorder.
        /// </summary>
        IRecordable Recordable { get; }
        /// <summary>
        /// Records, new records are added to.
        /// </summary>
        /// <value>Non-null between <see cref="Start"/> and <see cref="Stop"/>.</value>
        IRecords Records { get; }

        /// <summary>
        /// Starts recording.
        /// </summary>
        /// <param name="records">See <see cref="Records"/>.</param>
        void Start(IRecords records);
        /// <summary>
        /// Stops started recording.
        /// </summary>
        void Stop();
    }
}
