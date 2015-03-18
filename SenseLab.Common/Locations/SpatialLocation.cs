using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    [KnownType(typeof(Point))]
    [KnownType(typeof(Uri))]
    [KnownType(typeof(SpatialTextLocation))]
    [KnownType(typeof(SpatialLocationGroup))]
    public abstract class SpatialLocation :
        Location,
        ISpatialLocation
    {
        public new ISpatialLocation Clone()
        {
            return (ISpatialLocation)base.Clone();
        }
    }
}
