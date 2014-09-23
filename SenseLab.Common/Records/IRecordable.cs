using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Recordable object.
    /// </summary>
    public interface IRecordable :
        IId<Guid>
    {
        /// <summary>
        /// Record type.
        /// </summary>
        IRecordType Type { get; }

        /// <summary>
        /// Creates recorder.
        /// </summary>
        /// <param name="records">Records, new records are added to.</param>
        /// <param name="location">Optional spatial location (project node) used in records creation which can vary over time.</param>
        IRecorder CreateRecorder(IRecords records, ILocatable<ISpatialLocation> location = null);
    }
}
