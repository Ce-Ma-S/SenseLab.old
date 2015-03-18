using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class SamplingRecordProviderInfo :
        RecordProviderInfo
    {
        internal SamplingRecordProviderInfo(int id, Guid recordableId, bool isStarted, bool isPaused,
            bool recordPeriodEnabled, TimeSpan recordPeriod)
            : base(id, recordableId, isStarted, isPaused)
        {
            RecordPeriodEnabled = recordPeriodEnabled;
            RecordPeriod = recordPeriod;
        }

        [DataMember]
        public bool RecordPeriodEnabled { get; private set; }
        [DataMember]
        public TimeSpan RecordPeriod { get; private set; }
    }
}
