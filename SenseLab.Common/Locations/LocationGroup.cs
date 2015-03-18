using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    public class LocationGroup<T> :
        Location,
        ILocationGroup
        where T : ILocation
    {
        public LocationGroup(IList<T> locations = null)
        {
            if (locations == null)
                locations = new ObservableCollection<T>();
            Locations = locations;
        }

        [DataMember]
        public IList<T> Locations
        {
            get;
            private set;
        }
        IEnumerable<ILocation> ILocationGroup.Locations
        {
            get { return Locations.Cast<ILocation>(); }
        }

        public override ILocation Clone()
        {
            var clone = (LocationGroup<T>)base.Clone();
            clone.Locations = new ObservableCollection<T>(Locations.Select(l => (T)l.Clone()));
            return clone;
        }
        public override string ToString()
        {
            return string.Join("\n", Locations);
        }
    }
}
