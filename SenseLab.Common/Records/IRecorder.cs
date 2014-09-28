using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Allows recording of changes or current states.
    /// By subscribing to this recorder, recording starts and records created are available for watching observers until they unsubscribe.
    /// </summary>
    public interface IRecorder :
        IObservable<IRecord>,
        IDisposable
    {
        /// <summary>
        /// Whether recording is started by at least one active observer subscription.
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
        /// Sequence number of next record.
        /// </summary>
        uint NextSequenceNumber { get; }
        /// <summary>
        /// Optional spatial location (project node) used in records creation which can vary over time.
        /// </summary>
        ILocatable<ISpatialLocation> Location { get; }
    }
}
