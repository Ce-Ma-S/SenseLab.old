using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    public class LocatableValue<T> :
        NotifyPropertyChange,
        ILocatableValue<T>
    {
        public LocatableValue(
            T value,
            ITimeInterval temporalLocation = null,
            ISpatialLocation spatialLocation = null)
        {
            Value = value;
            TemporalLocation = temporalLocation;
            SpatialLocation = spatialLocation;
        }

        public static implicit operator LocatableValue<T>(T value)
        {
            return new LocatableValue<T>(value);
        }
        public static explicit operator T(LocatableValue<T> value)
        {
            value.ValidateNonNull(nameof(value));
            return value.Value;
        }

        #region Value

        public T Value
        {
            get { return value; }
            set { SetProperty(() => Value, ref this.value, value); }
        }
        object ILocatableValue.Value
        {
            get { return Value; }
        }

        [DataMember(Name = "Value")]
        private T value;

        #endregion

        #region ILocatable

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
            get { return false; }
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

        #endregion

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            TemporalLocationChanged = null;
            SpatialLocationChanged = null;
        }
    }
}
