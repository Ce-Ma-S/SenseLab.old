using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Records
{
    public abstract class Recordable<T> :
        RecordSource,
        IRecordable
        where T : IRecorder
    {
        public Recordable(Guid id, IRecordType type, string name, string description = null)
            : base(id, type, name, description)
        {
        }

        public abstract T CreateRecorder(
            IRecordGroup group,
            uint nextSequenceNumber,
            ILocatable<ISpatialLocation> spatialLocation);
        IRecorder IRecordable.CreateRecorder(
            IRecordGroup group,
            uint nextSequenceNumber,
            ILocatable<ISpatialLocation> spatialLocation)
        {
            return CreateRecorder(group, nextSequenceNumber ,spatialLocation);
        }
    }
}
