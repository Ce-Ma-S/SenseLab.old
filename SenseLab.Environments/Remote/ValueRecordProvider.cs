using SenseLab.Common.Records;
using SenseLab.Common.Values;
using SenseLab.Environments.Service;
using System;

namespace SenseLab.Environments.Remote
{
    public class ValueRecordProvider :
        SamplingRecordProvider,
        IValueRecordProvider
    {
        internal ValueRecordProvider(Environment environment, IEnvironmentService service,
            IRecordable recordable, int id, bool recordPeriodEnabled, TimeSpan recordPeriod,
            int batchSize, TimeSpan? batchPeriod)
            : base(environment, service, recordable, id, recordPeriodEnabled, recordPeriod)
        {
            this.batchSize = batchSize;
            this.batchPeriod = batchPeriod;
        }

        public int BatchSize
        {
            get { return batchSize; }
            set
            {
                Service.ValueRecordProvider_SetBatchSize(Id, value).Wait();
                batchSize = value;
            }
        }
        public TimeSpan? BatchPeriod
        {
            get { return batchPeriod; }
            set
            {
                Service.ValueRecordProvider_SetBatchPeriod(Id, value).Wait();
                batchPeriod = value;
            }
        }

        private int batchSize;
        private TimeSpan? batchPeriod;
    }
}
