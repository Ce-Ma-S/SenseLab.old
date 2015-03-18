using SenseLab.Common.Records;
using SenseLab.Environments.Service;
using System;
using System.Threading.Tasks;
using CeMaS.Data.Streams;
using CeMaS.Common.Validation;

namespace SenseLab.Environments.Remote
{
    public class StreamingRecordProvider :
        RecordProvider,
        IStreamingRecordProvider
    {
        internal StreamingRecordProvider(Environment environment, IEnvironmentService service,
            IRecordable recordable, int id, Guid streamManagerId)
            : base(environment, service, recordable, id)
        {
            streamManager = StreamManagers.Instance.GetItem(streamManagerId);
        }

        public IStreamManager StreamManager
        {
            get { return streamManager; }
        }

        public async Task SetStreamManager(IStreamManager value)
        {
            value.ValidateNonNull(nameof(value));
            await Service.StreamingRecordProvider_SetStreamManagerId(Id, value.Id);
            streamManager = value;
        }

        private IStreamManager streamManager;
    }
}
