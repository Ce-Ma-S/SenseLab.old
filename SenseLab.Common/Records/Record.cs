using SenseLab.Common.Environments;
using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public abstract class Record :
        NotifyPropertyChange,
        IRecord
    {
        public Record(
            Guid sourceId,
            uint sequenceNumber,
            ITime temporalLocation,
            ISpatialLocation spatialLocation = null)
        {
            temporalLocation.ValidateNonNull("temporalLocation");
            SourceId = sourceId;
            SequenceNumber = sequenceNumber;
            TemporalLocation = temporalLocation;
            SpatialLocation = spatialLocation;
        }

        public KeyValuePair<Guid, uint> Id
        {
            get { return new KeyValuePair<Guid, uint>(SourceId, SequenceNumber); }
        }
        public string Text
        {
            get { return GetText(); }
        }
        [DataMember]
        public Guid SourceId { get; private set; }
        [DataMember(Name = "Number")]
        public uint SequenceNumber { get; private set; }
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
        public Guid GroupId
        {
            get { return groupId; }
            set
            {
                SetProperty(() => GroupId, ref groupId, value);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}, {2})", Text, TemporalLocation, SpatialLocation);
        }

        protected abstract string GetText();

        [DataMember(Name = "Time")]
        private ITime temporalLocation;
        [DataMember(Name = "Space")]
        private ISpatialLocation spatialLocation;
        [DataMember(Name = "GroupId")]
        private Guid groupId;
    }
}
