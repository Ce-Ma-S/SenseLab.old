using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    public class SpatialLocationGroup<T> :
        LocationGroup<T>,
        ISpatialLocation
        where T : ISpatialLocation
    {
        public SpatialLocationGroup(IList<T> locations = null)
            : base(locations)
        {
        }

        public new ISpatialLocation Clone()
        {
            return (ISpatialLocation)base.Clone();
        }
    }


    [DataContract]
    public class SpatialLocationGroup :
        SpatialLocationGroup<ISpatialLocation>
    {
        public SpatialLocationGroup(IList<ISpatialLocation> locations = null)
            : base(locations)
        {
        }
    }
}
