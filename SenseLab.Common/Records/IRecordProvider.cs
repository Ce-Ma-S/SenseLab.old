using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Provides records of changes or current state of a recordable.
    /// By subscribing to this provider, records are provided (when the provider is started and not paused) and records created are available for watching observers until they unsubscribe.
    /// </summary>
    public interface IRecordProvider :
        IObservable<IRecord>,
        IDisposable
    {
        /// <summary>
        /// Recordable this provider monitors.
        /// </summary>
        IRecordable Recordable { get; }
        /// <summary>
        /// Whether record providing is started by <see cref="Start"/>.
        /// </summary>
        bool IsStarted { get; }
        /// <summary>
        /// Whether record providing started by <see cref="Start"/> is paused by <see cref="Pause"/>.
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Starts records providing.
        /// </summary>
        /// <remarks>Subscribe to this provider before calling this method.</remarks>
        /// <exception cref="InvalidOperationException">The provider is already started or nobody is subscribed to observe this provider.</exception>
        Task Start();
        /// <summary>
        /// Stops records providing.
        /// </summary>
        /// <remarks>Unsubscribing all observers from this provider calls this method automatically.</remarks>
        /// <exception cref="InvalidOperationException">The provider is already stopped.</exception>
        Task Stop();
        /// <summary>
        /// Pauses records providing started by <see cref="Start"/>.
        /// </summary>
        Task Pause();
        /// <summary>
        /// Unpauses records providing paused by <see cref="Pause"/>.
        /// </summary>
        Task Unpause();
    }
}