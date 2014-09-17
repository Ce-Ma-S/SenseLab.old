using SenseLab.Common.Events;
using System;

namespace SenseLab.Common.Records
{
    public abstract class SamplingRecorder<T> :
        Recorder<T>,
        ISamplingRecorder
        where T : class, IRecord
    {
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

        private void ApplyRecordPeriod()
        {
            if (RecordPeriodEnabled && RecordPeriod.Milliseconds > 0)
                StartSampling();
            else
                StopSampling();
        }
        private void StartSampling()
        {
            throw new NotImplementedException();
        }
        private void StopSampling()
        {
            throw new NotImplementedException();
        }

        private bool recordPeriodEnabled;
        private TimeSpan recordPeriod;
    }
}
