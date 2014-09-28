using SenseLab.Common.Locations;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecord :
        IId<KeyValuePair<Guid, uint>>,
        ILocatable<ISpatialLocation>,
        ILocatable<ITime>
    {
        string Text { get; }
        IRecordSource Source { get; }
        uint SequenceNumber { get; }
        IRecordGroup Group { get; }
        ISpatialLocation SpatialLocation { get; }
        ITime TemporalLocation { get; }
    }
}
