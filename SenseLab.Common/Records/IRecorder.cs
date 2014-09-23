using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Allows recording of changes or current states.
    /// </summary>
    public interface IRecorder :
        IDisposable
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
        IRecords Records { get; }
        /// <summary>
        /// Optional spatial location (project node) used in records creation which can vary over time.
        /// </summary>
        ILocatable<ISpatialLocation> Location { get; }

        /// <summary>
        /// Starts recording.
        /// </summary>
        void Start();
        /// <summary>
        /// Stops started recording.
        /// </summary>
        void Stop();
    }
}
