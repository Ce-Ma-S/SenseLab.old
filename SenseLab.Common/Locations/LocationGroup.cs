using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Locations
{
    public class LocationGroup<T> : Location, ILocationGroup
        where T : ILocation
    {
        public LocationGroup(IList<T> locations = null)
        {
            if (locations == null)
                locations = new List<T>();
            Locations = locations;
        }

        public IList<T> Locations
        {
            get;
            private set;
        }
        IEnumerable<ILocation> ILocationGroup.Locations
        {
            get { return Locations.Cast<ILocation>(); }
        }

        protected override string GetText()
        {
            return string.Join("\n", Locations);
        }
    }
}
