using CeMaS.Common;
using SenseLab.Common.Values;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    [KnownType(typeof(SamplingRecordProviderInfo))]
    public class RecordProviderInfo :
        IId<int>
    {
        protected RecordProviderInfo(int id, Guid recordableId, bool isStarted, bool isPaused)
        {
            Id = id;
            RecordableId = recordableId;
            IsStarted = IsStarted;
            IsPaused = isPaused;
        }

        public static RecordProviderInfo Create(int id, IRecordProvider recordProvider)
        {
            if (recordProvider is IValueRecordProvider)
            {
                var valueRecordProvider = (IValueRecordProvider)recordProvider;
                return new ValueRecordProviderInfo(id, recordProvider.Recordable.Id, recordProvider.IsStarted, recordProvider.IsPaused,
                    valueRecordProvider.RecordPeriodEnabled, valueRecordProvider.RecordPeriod,
                    valueRecordProvider.BatchSize, valueRecordProvider.BatchPeriod);
            }
            else if (recordProvider is ISamplingRecordProvider)
            {
                var samplingRecordProvider = (ISamplingRecordProvider)recordProvider;
                return new SamplingRecordProviderInfo(id, recordProvider.Recordable.Id, recordProvider.IsStarted, recordProvider.IsPaused,
                    samplingRecordProvider.RecordPeriodEnabled, samplingRecordProvider.RecordPeriod);
            }
            else if (recordProvider is IStreamingRecordProvider)
            {
                var streamingRecordProvider = (IStreamingRecordProvider)recordProvider;
                return new StreamingRecordProviderInfo(id, recordProvider.Recordable.Id, recordProvider.IsStarted, recordProvider.IsPaused,
                    streamingRecordProvider.StreamManager.Id);
            }
            return new RecordProviderInfo(id, recordProvider.Recordable.Id, recordProvider.IsStarted, recordProvider.IsPaused);
        }

        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public Guid RecordableId { get; private set; }
        [DataMember]
        public bool IsStarted { get; private set; }
        [DataMember]
        public bool IsPaused { get; private set; }
    }
}
