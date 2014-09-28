using SenseLab.Common.Locations;
using System;
using System.Timers;

namespace SenseLab.Common.Records
{
    public abstract class SamplingRecorder<T> :
        Recorder<T>,
        ISamplingRecorder
        where T : class, IRecord
    {
        public SamplingRecorder(uint nextSequenceNumber, ILocatable<ISpatialLocation> location)
            : base(nextSequenceNumber, location)
        {
        }

        public bool RecordPeriodEnabled
        {
            get
            {
                return recordPeriodEnabled;
            }
            set
            {
                if (SetProperty(() => RecordPeriodEnabled, ref recordPeriodEnabled, value))
                    ApplyRecordPeriod();
            }
        }
        public TimeSpan RecordPeriod
        {
            get
            {
                return recordPeriod;
            }
            set
            {
                if (SetProperty(() => RecordPeriod, ref recordPeriod, value))
                    ApplyRecordPeriod();
            }
        }

        public IRecord AddRecord()
        {
            return AddRecord(null);
        }

        protected override void DoStart()
        {
            ApplyRecordPeriod();
        }
        protected override void DoStop()
        {
            StopSampling();
        }

        private void ApplyRecordPeriod()
        {
            if (!IsStarted)
                return;
            if (RecordPeriodEnabled && RecordPeriod.Milliseconds > 0)
                StartSampling();
            else
                StopSampling();
        }
        private void StartSampling()
        {
            if (timer == null)
            {
                timer = new Timer() { AutoReset = true };
                timer.Elapsed += OnTime;
            }
            timer.Interval = RecordPeriod.Milliseconds;
            timer.Enabled = true;
        }
        private void StopSampling()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        private void OnTime(object sender, ElapsedEventArgs e)
        {
            AddRecord();
        }

        private bool recordPeriodEnabled;
        private TimeSpan recordPeriod;
        private Timer timer;
    }
}
