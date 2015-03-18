using CeMaS.Common.Events;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    [KnownType(typeof(SpatialLocation))]
    [KnownType(typeof(TemporalLocation))]
    public abstract class Location :
        NotifyPropertyChange,
        ILocation
    {
        public event EventHandler Changed;

        public virtual ILocation Clone()
        {
            var clone = (Location)MemberwiseClone();
            clone.ClearEventHandlers();
            return clone;
        }

        protected virtual void OnChanged()
        {
            Changed.RaiseEvent(this);
        }

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            Changed = null;
        }
    }
}
