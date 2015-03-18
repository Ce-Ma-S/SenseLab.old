using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class StreamingRecordProviderInfo :
        RecordProviderInfo
    {
        internal StreamingRecordProviderInfo(int id, Guid recordableId, bool isStarted, bool isPaused,
            Guid streamManagerId)
            : base(id, recordableId, isStarted, isPaused)
        {
            StreamManagerId = streamManagerId;
        }

        [DataMember]
        public Guid StreamManagerId { get; private set; }
    }
}
