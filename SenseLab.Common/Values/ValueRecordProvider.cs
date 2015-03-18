using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace SenseLab.Common.Values
{
    public class ValueRecordProvider<T> :
        SamplingRecordProvider,
        IValueRecordProvider
    {
        public ValueRecordProvider(IValue<T> value)
            : base(value)
        {
            batch = new List<LocatableValue<T>>();
        }

        public IValue<T> Value
        {
            get { return (IValue<T>)Recordable; }
        }
        public int BatchSize
        {
            get { return batchSize; }
            set
            {
                if (SetProperty(() => BatchSize, ref batchSize, value,
                    beforeChange: (name, v) => v.ValidateGreaterThan(0, name)))
                {
                    ApplyBatch();
                }
            }
        }
        public TimeSpan? BatchPeriod
        {
            get { return batchPeriod; }
            set
            {
                if (SetProperty(() => BatchPeriod, ref batchPeriod, value,
                    beforeChange: (name, v) =>
                    {
                        if (v.HasValue)
                            v.Value.ValidateGreaterThan(TimeSpan.Zero, name);
                    }))
                {
                    ApplyBatch();
                }
            }
        }

        protected override async Task DoStart()
        {
            await base.DoStart();
            Value.ValueChanged += OnValueChanged;
        }
        protected override async Task DoStop()
        {
            Value.ValueChanged -= OnValueChanged;
            await Task.Yield();
        }
        protected override async Task<IRecord> CreateRecord(object data)
        {
            var a = (ValueChangeEventArgs<T>)data;
            T value;
            if (a != null)
            {
                value = a.NewValue.Value;
            }
            else
            {
                await Value.ReadValue();
                value = Value.Value;
            }
            return Batch(value);
        }

        private IRecord Batch(T value)
        {
            if (batchTimer != null && batch.Count == 0)
                batchTimer.Start();
            batch.Add(
                new LocatableValue<T>(value, TimeInterval.Now));
            return TryReleaseBatch();
        }
        private IRecord TryReleaseBatch()
        {
            int count = batch.Count;
            if (count == 0)
                return null;
            if (count >= BatchSize)
                return ReleaseBatch();
            if (BatchPeriod.HasValue)
            {
                var period = DateTimeOffset.Now - batch[0].TemporalLocation.From;
                if (period >= BatchPeriod.Value)
                    return ReleaseBatch();
            }
            return null;
        }
        private IRecord ReleaseBatch()
        {
            if (batchTimer != null)
                batchTimer.Stop();
            int count = batch.Count;
            IRecord record;
            Debug.Assert(count > 0);
            if (count == 1)
            {
                var value = batch[0];
                record = new ValueRecord<T>(
                    0, Recordable.Id, value.Value, value.TemporalLocation, value.SpatialLocation);
            }
            else
            {
                Debug.Assert(count > 1);
                record = new ValuesRecord<T>(
                    0, Recordable.Id, batch);
            }
            batch.Clear();
            return record;
        }
        private void ApplyBatch()
        {
            TryReleaseBatchAddRecord();
            if (BatchPeriod.HasValue)
            {
                if (batchTimer == null)
                {
                    batchTimer = new Timer()
                    {
                        AutoReset = false
                    };
                    batchTimer.Elapsed += OnBatchPeriod;
                }
                batchTimer.Interval = BatchPeriod.Value.TotalMilliseconds;
            }
            else
            {
                if (batchTimer != null)
                {
                    batchTimer.Dispose();
                    batchTimer = null;
                }
            }
        }
        private void TryReleaseBatchAddRecord()
        {
            var record = TryReleaseBatch();
            if (record != null)
                AddRecord(record).Wait();
        }

        private void OnBatchPeriod(object sender, ElapsedEventArgs e)
        {
            TryReleaseBatchAddRecord();
        }
        private async void OnValueChanged(object sender, ValueChangeEventArgs<T> e)
        {
            await AddRecord(e);
        }

        private int batchSize;
        private TimeSpan? batchPeriod;
        private readonly List<LocatableValue<T>> batch;
        private Timer batchTimer;
    }
}
