using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    [KnownType(typeof(TimeInterval))]
    public abstract class TemporalLocation :
        Location,
        ITemporalLocation
    {
        public new ITemporalLocation Clone()
        {
            return (ITemporalLocation)base.Clone();
        }
    }
}
