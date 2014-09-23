using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Records
{
    public abstract class Recorder<T> :
        NotifyPropertyChange,
        IRecorder
        where T : class, IRecord
    {
        public Recorder(IRecords records, ILocatable<ISpatialLocation> location)
        {
            records.ValidateNonNull("records");
            if (records.Storage.IsReadOnly)
                throw new ArgumentException("Record storage is read only.", "records.Storage");
            if (records.Storage.IsConnected)
                throw new ArgumentException("Record storage is not connected.", "records.Storage");
            Records = records;
            Location = location;
        }
        public bool IsStarted
        {
            get { return Records != null; }
            private set
            {
                SetProperty(() => IsStarted, ref isStarted, value, OnIsStartedChanged);
            }
        }
        public bool IsPaused
        {
            get { return isPaused; }
            set
            {
                SetProperty(() => IsPaused, ref isPaused, value, OnIsPausedChanged);
            }
        }

        public abstract IRecordable Recordable { get; }
        public IRecords Records { get; private set; }
        public ILocatable<ISpatialLocation> Location { get; private set; }

        public void Start()
        {
            if (IsStarted)
                throw new InvalidOperationException("Recorder is already started.");
            doStartCalledAfterStart = false;
            if (Location is ILocatableChangeable<ISpatialLocation>)
            {
                ((ILocatableChangeable<ISpatialLocation>)Location).LocationChanged += OnSpatialLocationChanged;
                var changeableLocation = Location.Location as IChangeable;
                if (changeableLocation != null)
                {
                    changeableLocation.Changed += OnSpatialLocationChanged;
                }
            }
            if (!IsPaused)
            {
                DoStart();
                doStartCalledAfterStart = true;
            }
            IsStarted = true;
        }
        public void Stop()
        {
            if (!IsStarted)
                throw new InvalidOperationException("Recorder is already stopped.");
            if (Location is ILocatableChangeable<ISpatialLocation>)
            {
                ((ILocatableChangeable<ISpatialLocation>)Location).LocationChanged -= OnSpatialLocationChanged;
                var changeableLocation = Location.Location as IChangeable;
                if (changeableLocation != null)
                {
                    changeableLocation.Changed -= OnSpatialLocationChanged;
                }
            }
            if (doStartCalledAfterStart)
                DoStop();
            IsStarted = false;
        }

        protected abstract void DoStart();
        protected abstract void DoStop();
        protected abstract void DoPause();
        protected abstract void DoUnpause();
        protected abstract T CreateRecord(object data, ISpatialLocation spatialLocation);
        protected T AddRecord(object data)
        {
            if (!IsStarted)
                throw new InvalidOperationException("Recorder is not started.");
            if (IsPaused)
                return null;
            var record = CreateRecord(data, SpatialLocationClone);
            if (record != null)
                Records.Storage.AddOrReplace(record);
            return record;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && IsStarted)
            {
                Stop();
            }
        }

        protected virtual void OnIsStartedChanged()
        {
        }
        protected virtual void OnIsPausedChanged()
        {
            if (!IsStarted)
                return;
            if (IsPaused)
                DoPause();
            else if (doStartCalledAfterStart)
                DoUnpause();
            else
            {
                DoStart();
                doStartCalledAfterStart = true;
            }
        }

        private ISpatialLocation SpatialLocationClone
        {
            get
            {
                if (spatialLocationClone == null && Location != null)
                {
                    var spatialLocation = Location.Location;
                    if (spatialLocation != null)
                        spatialLocationClone = spatialLocation.Clone();
                }
                return spatialLocationClone;
            }
        }

        private void OnSpatialLocationChanged(object sender, ValueChangeEventArgs<ISpatialLocation> e)
        {
            spatialLocationClone = null;
            if (e.OldValue.HasValue)
            {
                var changeableLocation = e.OldValue.Value as IChangeable;
                if (changeableLocation != null)
                {
                    changeableLocation.Changed -= OnSpatialLocationChanged;
                }
            }
            {
                var changeableLocation = e.NewValue as IChangeable;
                if (changeableLocation != null)
                {
                    changeableLocation.Changed -= OnSpatialLocationChanged;
                }
            }
        }
        private void OnSpatialLocationChanged(object sender, EventArgs e)
        {
            spatialLocationClone = null;
        }

        private ISpatialLocation spatialLocationClone;
        private bool doStartCalledAfterStart;
        private bool isStarted;
        private bool isPaused;
    }
}
