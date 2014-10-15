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
        Guid SourceId { get; }
        uint SequenceNumber { get; }
        Guid GroupId { get; }
        ISpatialLocation SpatialLocation { get; }
        /// <summary>
        /// Temporal location.
        /// </summary>
        /// <value>non-null</value>
        ITime TemporalLocation { get; }
    }
}
