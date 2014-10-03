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
            IRecordSource source,
            uint sequenceNumber,
            ISpatialLocation spatialLocation = null,
            ITime temporalLocation = null)
        {
            source.ValidateNonNull("source");
            Source = source;
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
        public IRecordSource Source { get; private set; }
        [DataMember(Name = "Number")]
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

        [DataMember]
        private RecordSourceInfo SourceInfo
        {
            get { return new RecordSourceInfo(Source); }
            set
            {
                if (value.IsRecordable)
                    Source = EnvironmentHelper.RecordableFromId(value.Id);
                else
                    Source = RecordSourcesNonRecordable.Instance.TryGetFromId(value.Id);
                if (Source == null)
                    Source = new RecordSourceUnavailable(value);
            }
        }

        [DataMember(Name = "Space")]
        private ISpatialLocation spatialLocation;
        [DataMember(Name = "Time")]
        private ITime temporalLocation;
        [DataMember(Name = "Group")]
        private IRecordGroup group;
    }
}
