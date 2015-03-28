using CeMaS.Common.Collections;
using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenseLab.Environments.Common
{
    public abstract class Device :
        DeviceProvider,
        IDevice
    {
        public Device(Guid id, string name, string description = null, ISpatialLocation location = null)
            : base(id, name, description)
        {
            Location = location;
            Recordables = new ObservableCollectionEx<IRecordable, Guid>();
            Recordables.Added.Subscribe(OnRecordablesAdded);
            Recordables.Removed.Subscribe(OnRecordablesRemoved);
        }

        #region Recordables

        public INotifyList<IRecordable, Guid> Recordables { get; private set; }
        INotifyEnumerable<IRecordable, Guid> IDevice.Recordables
        {
            get { return Recordables; }
        }

        protected virtual void OnRecordablesAdded(IEnumerable<IRecordable> items)
        {
        }
        protected virtual void OnRecordablesRemoved(IEnumerable<IRecordable> items)
        {
        }

        #endregion

        #region Available

        public bool IsAvailable
        {
            get { return GetIsAvailable(); }
        }
        public event EventHandler IsAvailableChanged;

        protected abstract bool GetIsAvailable();

        protected virtual void OnIsAvailableChanged()
        {
            IsAvailableChanged.RaiseEvent(this);
        }

        #endregion

        #region IConnectable

        public bool IsConnected
        {
            get { return GetIsConnected(); }
        }
        public event EventHandler IsConnectedChanged;

        public async Task Connect()
        {
            if (IsConnected)
                return;
            await DoConnect();
        }
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;
            await DoDisconnect();
        }

        protected abstract bool GetIsConnected();
        protected abstract Task DoConnect();
        protected abstract Task DoDisconnect();

        protected virtual void OnIsConnectedChanged()
        {
            IsConnectedChanged.RaiseEvent(this);
        }

        #endregion

        #region ILocatable

        public ISpatialLocation Location
        {
            get { return location; }
            set
            {
                if (LocationIsRequired)
                    value.ValidateNonNull(nameof(Location));
                SetProperty(() => Location, ref location, value, OnLocationChanged);
            }
        }
        public event EventHandler<ValueChangeEventArgs<ISpatialLocation>> LocationChanged;

        public virtual bool LocationIsRequired
        {
            get { return false; }
        }

        protected virtual void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            if (oldValue != null)
                oldValue.Changed -= OnLocationChanged;
            if (newValue != null)
                newValue.Changed += OnLocationChanged;
            LocationChanged.RaiseEvent(this, () => new ValueChangeEventArgs<ISpatialLocation>(oldValue, newValue));
        }
        protected virtual void OnLocationChanged(ISpatialLocation location)
        {
        }

        private void OnLocationChanged(object sender, EventArgs e)
        {
            OnLocationChanged((ISpatialLocation)sender);
        }

        private ISpatialLocation location;

        #endregion

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            if (Location != null)
                Location.Changed -= OnLocationChanged;
        }
    }
}
