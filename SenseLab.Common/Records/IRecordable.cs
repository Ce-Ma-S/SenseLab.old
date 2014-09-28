using SenseLab.Common.Locations;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Recordable object.
    /// </summary>
    public interface IRecordable :
        IRecordSource
    {
        /// <summary>
        /// Creates recorder.
        /// </summary>
        /// <param name="group">Optional group for new records.</param>
        /// <param name="nextSequenceNumber">Sequence number of next new record.</param>
        /// <param name="location">Optional spatial location (project node) used in records creation which can vary over time.</param>
        IRecorder CreateRecorder(
            IRecordGroup group = null,
            uint nextSequenceNumber = 0,
            ILocatable<ISpatialLocation> location = null);
    }
}
