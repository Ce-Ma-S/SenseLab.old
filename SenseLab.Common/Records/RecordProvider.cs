using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    public abstract class RecordProvider :
        NotifyPropertyChange,
        IRecordProvider
    {
        public RecordProvider(IRecordable recordable)
        {
            recordable.ValidateNonNull(nameof(recordable));
            Recordable = recordable;
            records = new ReplaySubject<IRecord>(1);
        }

        public bool IsStarted
        {
            get { return isStarted; }
            private set
            {
                SetProperty(() => IsStarted, ref isStarted, value, OnIsStartedChanged);
            }
        }
        public bool IsPaused
        {
            get { return isPaused; }
            private set
            {
                SetProperty(() => IsPaused, ref isPaused, value, OnIsPausedChanged);
            }
        }
        public IRecordable Recordable { get; private set; }

        public IDisposable Subscribe(IObserver<IRecord> observer)
        {
            var subscription = records.Subscribe(observer);
            return System.Reactive.Disposables.Disposable.Create(
                () =>
                {
                    subscription.Dispose();
                    if (!records.HasObservers && IsStarted)
                        Stop().Wait();
                });
        }
        public async Task Start()
        {
            if (IsStarted)
                throw new InvalidOperationException("Record provider is already started.");
            if (!records.HasObservers)
                throw new InvalidOperationException("Subscribe to this provider first before starting it.");
            doStartCalledAfterStart = false;
            if (!IsPaused)
            {
                await DoStart();
                doStartCalledAfterStart = true;
            }
            IsStarted = true;
        }
        public async Task Stop()
        {
            if (!IsStarted)
                throw new InvalidOperationException("Record provider is already stopped.");
            if (doStartCalledAfterStart)
                await DoStop();
            IsStarted = false;
        }
        public async Task Pause()
        {
            if (IsStarted && doStartCalledAfterStart)
                await DoPause();
            IsPaused = true;
        }
        public async Task Unpause()
        {
            if (IsStarted)
            {
                if (doStartCalledAfterStart)
                {
                    await DoUnpause();
                }
                else
                {
                    await DoStart();
                    doStartCalledAfterStart = true;
                }
            }
            IsPaused = false;
        }

        protected abstract Task DoStart();
        protected abstract Task DoStop();
        protected async virtual Task DoPause()
        {
            await DoStop();
        }
        protected async virtual Task DoUnpause()
        {
            await DoStart();
        }
        protected virtual Task<IRecord> CreateRecord(object data)
        {
            if (data is IRecord)
                return Task.FromResult((IRecord)data);
            else
                throw new NotImplementedException();
        }
        protected async Task<IRecord> AddRecord(object data)
        {
            if (!IsStarted || IsPaused)
                return null;
            var record = data is IRecord ?
                (IRecord)data :
                await CreateRecord(data);
            if (record != null)
                records.OnNext(record);
            return record;
        }
        protected override async void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (IsStarted)
                    await Stop();
                records.Dispose();
            }
        }

        protected virtual void OnIsStartedChanged()
        {
        }
        protected virtual void OnIsPausedChanged()
        {
        }

        private readonly ReplaySubject<IRecord> records;
        private bool doStartCalledAfterStart;
        private bool isStarted;
        private bool isPaused;
    }
}
