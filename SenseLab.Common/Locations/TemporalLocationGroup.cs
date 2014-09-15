using System.Collections.Generic;

namespace SenseLab.Common.Locations
{
    public class TemporalLocationGroup<T> :
        LocationGroup<T>,
        ITemporalLocation
        where T : ITemporalLocation
    {
        public TemporalLocationGroup(IList<T> locations = null)
            : base(locations)
        {
        }
    }
}
