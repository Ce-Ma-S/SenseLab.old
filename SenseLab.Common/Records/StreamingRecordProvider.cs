using CeMaS.Common.Validation;
using CeMaS.Data.Streams;
using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    public abstract class StreamingRecordProvider :
        RecordProvider,
        IStreamingRecordProvider
    {
        public StreamingRecordProvider(IRecordable recordable)
            : base(recordable)
        {
        }

        public IStreamManager StreamManager { get; private set; }

        public async Task SetStreamManager(IStreamManager value)
        {
            value.ValidateNonNull(nameof(value));
            SetProperty(() => StreamManager, v => StreamManager = v, value);
            await Task.Yield();
        }
    }
}
