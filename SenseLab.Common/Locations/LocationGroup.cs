using System.Collections.Generic;

namespace SenseLab.Common.Locations
{
    public class LocationGroup<T> : Location, ILocationGroup
        where T : ILocation
    {
        public LocationGroup(IList<ILocation> locations = null)
        {
            if (locations == null)
                locations = new List<ILocation>();
            Locations = locations;
        }

        public override string Text
        {
            get { return string.Join("\n", Locations); }
        }
        public IList<ILocation> Locations
        {
            get;
            private set;
        }
        IEnumerable<ILocation> ILocationGroup.Locations
        {
            get { return Locations; }
        }
    }
}
