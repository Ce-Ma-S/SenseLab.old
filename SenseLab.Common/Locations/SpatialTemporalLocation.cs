using System.Collections.Generic;
namespace SenseLab.Common.Locations
{
    public class SpatialTemporalLocation :
        Location, ISpatialTemporalLocation
    {
        public SpatialTemporalLocation(ISpatialLocation spatialLocation = null, ITemporalLocation temporalLocation = null)
        {
            SpatialLocation = spatialLocation;
            TemporalLocation = temporalLocation;
        }

        public ISpatialLocation SpatialLocation
        {
            get { return spatialLocation; }
            set
            {
                SetProperty(() => SpatialLocation, ref spatialLocation, value, OnTextChanged);
            }
        }
        public ITemporalLocation TemporalLocation
        {
            get { return temporalLocation; }
            set
            {
                SetProperty(() => TemporalLocation, ref temporalLocation, value, OnTextChanged);
            }
        }
        public IEnumerable<ILocation> Locations
        {
            get
            {
                if (SpatialLocation != null)
                    yield return SpatialLocation;
                if (TemporalLocation != null)
                    yield return TemporalLocation;
            }
        }

        protected override string GetText()
        {
            return string.Format("{0}\n{1}", SpatialLocation, TemporalLocation);
        }

        private ISpatialLocation spatialLocation;
        private ITemporalLocation temporalLocation;
    }
}
