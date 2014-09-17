using SenseLab.Common.Events;
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
                SetProperty(() => Records, ref records, value, OnRecordsChanged);
            }
        }

        public void Start(IRecords records)
        {
            records.ValidateNonNull("records");
            if (records.IsReadOnly)
                throw new ArgumentException("Records are read only.", "records");
            if (IsStarted)
                throw new InvalidOperationException("Recorder is already started.");
            Records = records;
            DoStart();
        }
        public void Stop()
        {
            if (!IsStarted)
                throw new InvalidOperationException("Recorder is already stopped.");
            Records = null;
            DoStop();
        }

        protected abstract void DoStart();
        protected abstract void DoStop();
        protected abstract void DoPause();
        protected abstract void DoUnpause();
        protected abstract T CreateRecord(object data);
        protected T AddRecord(object data)
        {
            if (!IsStarted)
                throw new InvalidOperationException("Recorder is not started.");
            if (IsPaused)
                return null;
            var record = CreateRecord(data);
            Records.Add(record);
            return record;
        }

        protected virtual void OnRecordsChanged(IRecords oldRecords, IRecords newRecords)
        {
            OnIsStartedChanged();
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
                DoUnpause();
            else
                DoPause();
        }
        
        private IRecords records;
        private bool isPaused;
    }
}
