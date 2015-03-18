using CeMaS.Common;
using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    [KnownType(typeof(Location))]
    public class LocatableItemInfo<TId, TLocation> :
        ItemInfo<TId>,
        ILocatable<TLocation>
        where TLocation : ILocation
    {
        public LocatableItemInfo(TId id, string name, string description = null, TLocation location = default(TLocation))
            : base(id, name, description)
        {
            Location = location;
        }

        #region ILocatable

        [DataMember]
        public TLocation Location
        {
            get { return location; }
            set
            {
                if (LocationIsRequired)
                    value.ValidateNonNull(nameof(Location));
                SetProperty(() => Location, ref location, value, OnLocationChanged);
            }
        }
        public event EventHandler<ValueChangeEventArgs<TLocation>> LocationChanged;
        public virtual bool LocationIsRequired
        {
            get { return false; }
        }

        protected virtual void OnLocationChanged(TLocation oldValue, TLocation newValue)
        {
            if (oldValue != null)
                oldValue.Changed -= OnLocationChanged;
            if (newValue != null)
                newValue.Changed += OnLocationChanged;
            LocationChanged.RaiseEvent(this, () => new ValueChangeEventArgs<TLocation>(oldValue, newValue));
        }
        protected virtual void OnLocationChanged(ISpatialLocation location)
        {
        }

        private void OnLocationChanged(object sender, EventArgs e)
        {
            OnLocationChanged((ISpatialLocation)sender);
        }

        private TLocation location;

        #endregion

        public override string ToString()
        {
            string text = base.ToString();
            if (Location != null)
                text += "\n" + Location;
            return text;
        }

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            LocationChanged = null;
        }
    }
}
