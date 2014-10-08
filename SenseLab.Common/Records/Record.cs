using SenseLab.Common.Data;
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
        IRecord,
        ISerializableSubitems
    {
        public Record(
            IRecordSource source,
            uint sequenceNumber,
            ITime temporalLocation,
            ISpatialLocation spatialLocation = null)
        {
            source.ValidateNonNull("source");
            temporalLocation.ValidateNonNull("temporalLocation");
            Source = source;
            sourceId = source.Id;
            SequenceNumber = sequenceNumber;
            TemporalLocation = temporalLocation;
            SpatialLocation = spatialLocation;
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
        public IRecordGroup Group
        {
            get { return group; }
            set
            {
                if (SetProperty(() => Group, ref group, value))
                {
                    groupId = value == null ? (Guid?)null : value.Id;
                }
            }
        }

        #region ISerializableSubitems

        public IEnumerable<SerializableItemInfo> SubitemInfos
        {
            get
            {
                yield return new SerializableItemInfo(sourceInfoName, typeof(RecordSourceInfo), Source.Id);
                if (groupId.HasValue)
                    yield return new SerializableItemInfo(groupName, typeof(IRecordGroup), groupId.Value);
            }
        }
        public object this[SerializableItemInfo subitemInfo]
        {
            get
            {
                switch (subitemInfo.Name)
                {
                    case sourceInfoName:
                        return SourceInfo;
                    case groupName:
                        return Group;
                    default:
                        return null;
                };
            }
            set
            {
                switch (subitemInfo.Name)
                {
                    case sourceInfoName:
                        SourceInfo = (RecordSourceInfo)value;
                        break;
                    case groupName:
                        Group = (IRecordGroup)value;
                        break;
                    default:
                        break;
                };
            }
        }

        private const string sourceInfoName = "SourceInfo";
        private const string groupName = "Group";

        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1}, {2})", Text, TemporalLocation, SpatialLocation);
        }

        protected abstract string GetText();

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

        [DataMember(Name = "SourceId")]
        private Guid sourceId;
        [DataMember(Name = "Time")]
        private ITime temporalLocation;
        [DataMember(Name = "Space")]
        private ISpatialLocation spatialLocation;
        [DataMember(Name = "GroupId")]
        private Guid? groupId;
        private IRecordGroup group;
    }
}
