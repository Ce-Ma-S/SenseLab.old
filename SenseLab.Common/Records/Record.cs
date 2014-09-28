using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public abstract class Record :
        NotifyPropertyChange,
        IRecord
    {
        public Record(
            uint sequenceNumber,
            ISpatialLocation spatialLocation = null,
            ITime temporalLocation = null)
        {
            SequenceNumber = sequenceNumber;
            SpatialLocation = spatialLocation;
            TemporalLocation = temporalLocation;
        }

        public KeyValuePair<Guid, uint> Id
        {
            get { return new KeyValuePair<Guid, uint>(Source.Id, SequenceNumber); }
        }
        public string Text
        {
            get { return GetText(); }
        }
        public abstract IRecordSource Source { get; }
        public uint SequenceNumber { get; private set; }
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
        public ITime TemporalLocation
        {
            get { return temporalLocation; }
            set
            {
                SetProperty(() => TemporalLocation, ref temporalLocation, value);
            }
        }
        ITime ILocatable<ITime>.Location
        {
            get { return TemporalLocation; }
        }
        public IRecordGroup Group
        {
            get { return group; }
            set
            {
                SetProperty(() => Group, ref group, value);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}, {2})", Text, TemporalLocation, SpatialLocation);
        }

        protected abstract string GetText();

        private ISpatialLocation spatialLocation;
        private ITime temporalLocation;
        private IRecordGroup group;        
    }
}
