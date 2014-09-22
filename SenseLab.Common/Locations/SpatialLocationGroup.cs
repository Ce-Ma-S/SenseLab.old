using System.Collections.Generic;

namespace SenseLab.Common.Locations
{
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
}
