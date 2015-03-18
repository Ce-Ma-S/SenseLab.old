using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class ValueRecordProviderInfo :
        SamplingRecordProviderInfo
    {
        internal ValueRecordProviderInfo(int id, Guid recordableId, bool isStarted, bool isPaused,
            bool recordPeriodEnabled, TimeSpan recordPeriod, int batchSize, TimeSpan? batchPeriod)
            : base(id, recordableId, isStarted, isPaused, recordPeriodEnabled, recordPeriod)
        {
            BatchSize = batchSize;
            BatchPeriod = batchPeriod;
        }

        [DataMember]
        public int BatchSize { get; private set; }
        [DataMember]
        public TimeSpan? BatchPeriod { get; private set; }
    }
}
