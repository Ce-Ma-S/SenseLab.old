using SenseLab.Common.Events;

namespace SenseLab.Common.Locations
{
    public class SpatialTextLocation :
        TextLocation, ISpatialLocation
    {
        public SpatialTextLocation(string text)
            : base(text)
        {
        }
    }
}
