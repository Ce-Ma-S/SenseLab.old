using CeMaS.Common;
using CeMaS.Common.Validation;
using SenseLab.Common.Records;
using SenseLab.Environments.Service;
using System.Threading.Tasks;

namespace SenseLab.Environments.Remote
{
    public class RecordProvider :
        SenseLab.Common.Records.RecordProvider,
        IId<int>
    {
        protected RecordProvider(Environment environment, IEnvironmentService service,
            IRecordable recordable, int id)
            : base(recordable)
        {
            this.environment = environment;
            Service = service;
            Id = id;
        }

        internal static async Task<RecordProvider> Create(IRecordable recordable, Environment environment, IEnvironmentService service)
        {
            service.ValidateNonNull(nameof(service));
            var info = await service.Recordable_CreateRecordProvider(recordable.Id);
            RecordProvider result;
            if (info is ValueRecordProviderInfo)
            {
                var valueInfo = (ValueRecordProviderInfo)info;
                result = new ValueRecordProvider(environment, service, recordable, info.Id,
                    valueInfo.RecordPeriodEnabled, valueInfo.RecordPeriod,
                    valueInfo.BatchSize, valueInfo.BatchPeriod);
            }
            else if (info is SamplingRecordProviderInfo)
            {
                var samplingInfo = (SamplingRecordProviderInfo)info;
                result = new SamplingRecordProvider(environment, service, recordable, info.Id,
                    samplingInfo.RecordPeriodEnabled, samplingInfo.RecordPeriod);
            }
            else if (info is StreamingRecordProviderInfo)
            {
                var streamingInfo = (StreamingRecordProviderInfo)info;
                result = new StreamingRecordProvider(environment, service, recordable, info.Id,
                    streamingInfo.StreamManagerId);
            }
            else
            {
                result = new RecordProvider(environment, service, recordable, info.Id);
            }
            environment.AddRecordProvider(result);
            return result;
        }

        public int Id { get; private set; }

        protected IEnvironmentService Service { get; private set; }

        internal async Task AddRecord(IRecord record)
        {
            await base.AddRecord(record);
        }

        protected override async Task DoStart()
        {
            await Service.RecordProvider_Start(Id);
        }
        protected override async Task DoStop()
        {
            await Service.RecordProvider_Stop(Id);
        }
        protected override async Task DoPause()
        {
            await Service.RecordProvider_Pause(Id);
        }
        protected override async Task DoUnpause()
        {
            await Service.RecordProvider_Unpause(Id);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                Service.RecordProvider_Dispose(Id);
                environment.RemoveRecordProvider(this);
            }
        }

        private readonly Environment environment;
    }
}
