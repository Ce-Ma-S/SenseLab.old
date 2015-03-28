using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using SenseLab.Common.Commands;
using SenseLab.Common.Locations;
using SenseLab.Common.Values;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    [KnownType(typeof(ValueRecord<>))]
    [KnownType(typeof(CommandRecord<>))]
    [KnownType(typeof(StreamableRecord))]
    public abstract class Record :
        NotifyPropertyChange,
        IRecord
    {
        public Record(
            uint id,
            Guid sourceId,
            ITimeInterval temporalLocation,
            ISpatialLocation spatialLocation = null)
        {
            Id = id;
            SourceId = sourceId;
            TemporalLocation = temporalLocation;
            SpatialLocation = spatialLocation;
        }

        #region Id

        public uint Id
        {
            get { return id; }
            set
            {
                SetProperty(() => Id, ref id, value, OnIdChanged);
            }
        }
        public event EventHandler IdChanged;

        protected virtual void OnIdChanged()
        {
            IdChanged.RaiseEvent(this);
        }

        [DataMember(Name = "Id")]
        private uint id;

        #endregion

        [DataMember]
        public Guid SourceId { get; private set; }

        #region TemporalLocation

        public ITimeInterval TemporalLocation
        {
            get { return temporalLocation; }
            set
            {
                SetProperty(() => TemporalLocation, ref temporalLocation, value, OnTemporalLocationChanged,
                    beforeChange: (name, v) => v.ValidateNonNull(name));
            }
        }
        public event EventHandler<ValueChangeEventArgs<ITimeInterval>> TemporalLocationChanged;
        ITimeInterval ILocatable<ITimeInterval>.Location
        {
            get { return TemporalLocation; }
            set { TemporalLocation = value; }
        }
        event EventHandler<ValueChangeEventArgs<ITimeInterval>> ILocatable<ITimeInterval>.LocationChanged
        {
            add { TemporalLocationChanged += value; }
            remove { TemporalLocationChanged -= value; }
        }
        bool ILocatable<ITimeInterval>.LocationIsRequired
        {
            get { return true; }
        }

        protected virtual void OnTemporalLocationChanged(ITimeInterval oldValue, ITimeInterval newValue)
        {
            TemporalLocationChanged.RaiseEvent(this, () => new ValueChangeEventArgs<ITimeInterval>(oldValue, newValue));
        }

        [DataMember(Name = "Time")]
        private ITimeInterval temporalLocation;

        #endregion

        #region SpatialLocation

        public ISpatialLocation SpatialLocation
        {
            get { return spatialLocation; }
            set
            {
                SetProperty(() => SpatialLocation, ref spatialLocation, value, OnSpatialLocationChanged);
            }
        }
        public event EventHandler<ValueChangeEventArgs<ISpatialLocation>> SpatialLocationChanged;
        ISpatialLocation ILocatable<ISpatialLocation>.Location
        {
            get { return SpatialLocation; }
            set { SpatialLocation = value; }
        }
        event EventHandler<ValueChangeEventArgs<ISpatialLocation>> ILocatable<ISpatialLocation>.LocationChanged
        {
            add { SpatialLocationChanged += value; }
            remove { SpatialLocationChanged -= value; }
        }
        bool ILocatable<ISpatialLocation>.LocationIsRequired
        {
            get { return false; }
        }

        protected virtual void OnSpatialLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            SpatialLocationChanged.RaiseEvent(this, () => new ValueChangeEventArgs<ISpatialLocation>(oldValue, newValue));
        }

        [DataMember(Name = "Space")]
        private ISpatialLocation spatialLocation;

        #endregion

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            IdChanged = null;
            TemporalLocationChanged = null;
            SpatialLocationChanged = null;
        }
    }
}
