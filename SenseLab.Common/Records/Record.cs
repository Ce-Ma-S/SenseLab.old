using SenseLab.Common.Events;
using SenseLab.Common.Locations;

namespace SenseLab.Common.Records
{
    public abstract class Record :
        NotifyPropertyChange,
        IRecord
    {
        public string Text
        {
            get { return GetText(); }
        }
        public IRecorder Recorder
        {
            get { return recorder; }
            set
            {
                SetProperty(() => Recorder, ref recorder, value);
            }
        }
        public ISpatialLocation SpatialLocation
        {
            get { return spatialLocation; }
            set
            {
                SetProperty(() => SpatialLocation, ref spatialLocation, value);
            }
        }
        ISpatialLocation ILocatable<ISpatialLocation>.Location
        {
            get { return SpatialLocation; }
        }
        public ITemporalLocation TemporalLocation
        {
            get { return temporalLocation; }
            set
            {
                SetProperty(() => TemporalLocation, ref temporalLocation, value);
            }
        }
        ITemporalLocation ILocatable<ITemporalLocation>.Location
        {
            get { return TemporalLocation; }
        }

        private ISpatialLocation spatialLocation;
        private ITemporalLocation temporalLocation;

        protected abstract string GetText();

        private IRecorder recorder;
    }
}
