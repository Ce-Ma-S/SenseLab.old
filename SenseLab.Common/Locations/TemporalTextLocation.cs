using SenseLab.Common.Events;

namespace SenseLab.Common.Locations
{
    public class TemporalTextLocation :
        TextLocation,
        ITemporalLocation
    {
        public TemporalTextLocation(string text)
            : base(text)
        {
        }

        public new ITemporalLocation Clone()
        {
            return (ITemporalLocation)base.Clone();
        }
    }
}
