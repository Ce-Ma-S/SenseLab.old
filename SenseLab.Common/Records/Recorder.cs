using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SenseLab.Common.Records
{
    public abstract class Recorder<T> :
        NotifyPropertyChange,
        IRecorder
        where T : class, IRecord
    {
        public Recorder(ILocatable<ISpatialLocation> location)
        {
            Location = location;
        }

        public bool IsStarted
        {
            get { return RecordsSubject != null; }
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
        public ILocatable<ISpatialLocation> Location { get; private set; }

        public IDisposable Subscribe(IObserver<IRecord> observer)
        {
            var subscription = recordsSubject.Subscribe(observer);
            if (!IsStarted)
                Start();
            return System.Reactive.Disposables.Disposable.Create(
                () =>
                {
                    subscription.Dispose();
                    if (!recordsSubject.HasObservers)
                        Stop();
                });
        }

        protected abstract void DoStart();
        protected abstract void DoStop();
        protected abstract void DoPause();
        protected abstract void DoUnpause();
        protected abstract T CreateRecord(object data, ISpatialLocation spatialLocation);
        protected T AddRecord(object data)
        {
            if (!IsStarted || IsPaused)
                return null;
            var record = CreateRecord(data, SpatialLocationClone);
            if (record != null)
                RecordsSubject.OnNext(record);
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
        private ReplaySubject<T> RecordsSubject
        {
            get { return recordsSubject; }
            set
            {
                recordsSubject = value;
                OnIsStartedChanged();
            }
        }

        private void Start()
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
            RecordsSubject = new ReplaySubject<T>(1);
        }
        private void Stop()
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
            RecordsSubject.Dispose();
            RecordsSubject = null;
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
        private bool isPaused;
        private ReplaySubject<T> recordsSubject;
    }
}
