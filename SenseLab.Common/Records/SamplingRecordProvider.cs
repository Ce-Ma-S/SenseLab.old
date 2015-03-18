using System;
using System.Threading.Tasks;
using System.Timers;

namespace SenseLab.Common.Records
{
    public abstract class SamplingRecordProvider :
        RecordProvider,
        ISamplingRecordProvider
    {
        public SamplingRecordProvider(IRecordable recordable)
            : base(recordable)
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
                    ApplyRecordPeriod(true);
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
                    ApplyRecordPeriod(true);
            }
        }

        public async Task<IRecord> AddRecord()
        {
            return await AddRecord(null);
        }

        protected override async Task DoStart()
        {
            ApplyRecordPeriod(false);
            await AddRecord();
        }
        protected override async Task DoStop()
        {
            StopSampling();
            await Task.Yield();
        }

        private void ApplyRecordPeriod(bool checkIsStarted)
        {
            if (checkIsStarted && !IsStarted)
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
                timer = new Timer();
                timer.Elapsed += OnTime;
            }
            timer.Interval = RecordPeriod.TotalMilliseconds;
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

        private async void OnTime(object sender, ElapsedEventArgs e)
        {
            await AddRecord();
        }

        private bool recordPeriodEnabled;
        private TimeSpan recordPeriod;
        private Timer timer;
    }
}
