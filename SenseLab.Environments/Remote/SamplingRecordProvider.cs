using SenseLab.Common.Records;
using SenseLab.Environments.Service;
using System;
using System.Threading.Tasks;

namespace SenseLab.Environments.Remote
{
    public class SamplingRecordProvider :
        RecordProvider,
        ISamplingRecordProvider
    {
        internal SamplingRecordProvider(Environment environment, IEnvironmentService service,
            IRecordable recordable, int id, bool recordPeriodEnabled, TimeSpan recordPeriod)
            : base(environment, service, recordable, id)
        {
            this.recordPeriodEnabled = recordPeriodEnabled;
            this.recordPeriod = recordPeriod;
        }

        public bool RecordPeriodEnabled
        {
            get { return recordPeriodEnabled; }
            set
            {
                Service.SamplingRecordProvider_SetRecordPeriodEnabled(Id, value).Wait();
                recordPeriodEnabled = value;
            }
        }
        public TimeSpan RecordPeriod
        {
            get { return recordPeriod; }
            set
            {
                Service.SamplingRecordProvider_SetRecordPeriod(Id, value).Wait();
                recordPeriod = value;
            }
        }

        public async Task<IRecord> AddRecord()
        {
            return await Service.SamplingRecordProvider_AddRecord(Id);
        }

        private bool recordPeriodEnabled;
        private TimeSpan recordPeriod;
    }
}
