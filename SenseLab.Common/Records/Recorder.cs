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
        public bool IsStarted
        {
            get { return Records != null; }
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
        public IRecords Records
        {
            get { return records; }
            private set
            {
                SetProperty(() => Records, ref records, value, OnIsStartedChanged);
            }
        }
        public ILocatable<ISpatialLocation> SpatialLocation
        {
            get { return spatialLocation; }
            private set
            {
                SetProperty(() => SpatialLocation, ref spatialLocation, value);
            }
        }

        public void Start(IRecords records, ILocatable<ISpatialLocation> spatialLocation)
        {
            records.ValidateNonNull("records");
            if (records.IsReadOnly)
                throw new ArgumentException("Records are read only.", "records");
            if (IsStarted)
                throw new InvalidOperationException("Recorder is already started.");
            doStartCalledAfterStart = false;
            Records = records;
            SpatialLocation = spatialLocation;
            if (spatialLocation is ILocatableChangeable<ISpatialLocation>)
            {
                ((ILocatableChangeable<ISpatialLocation>)spatialLocation).LocationChanged += OnSpatialLocationChanged;
                var changeableLocation = spatialLocation.Location as IChangeable;
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
        }
        public void Stop()
        {
            if (!IsStarted)
                throw new InvalidOperationException("Recorder is already stopped.");
            Records = null;
            if (spatialLocation is ILocatableChangeable<ISpatialLocation>)
            {
                ((ILocatableChangeable<ISpatialLocation>)spatialLocation).LocationChanged -= OnSpatialLocationChanged;
                var changeableLocation = spatialLocation.Location as IChangeable;
                if (changeableLocation != null)
                {
                    changeableLocation.Changed -= OnSpatialLocationChanged;
                }
            }
            SpatialLocation = null;
            if (doStartCalledAfterStart)
                DoStop();
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
            Records.Add(record);
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
            OnPropertyChanged(() => IsStarted);
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
                if (spatialLocationClone == null && SpatialLocation != null)
                {
                    var spatialLocation = SpatialLocation.Location;
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

        private IRecords records;
        private ILocatable<ISpatialLocation> spatialLocation;
        private ISpatialLocation spatialLocationClone;
        private bool doStartCalledAfterStart;
        private bool isPaused;
    }
}
