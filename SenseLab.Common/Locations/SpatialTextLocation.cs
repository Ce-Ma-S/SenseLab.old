using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    public class SpatialTextLocation :
        TextLocation,
        ISpatialLocation
    {
        public SpatialTextLocation(string text)
            : base(text)
        {
        }

        public new ISpatialLocation Clone()
        {
            return (ISpatialLocation)base.Clone();
        }
    }
}
