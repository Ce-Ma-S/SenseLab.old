﻿using SenseLab.Common.Events;
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
        public IRecordable Recordable
        {
            get { return recordable; }
            set
            {
                SetProperty(() => Recordable, ref recordable, value);
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

        private IRecordable recordable;
    }
}
