using CeMaS.Common.Validation;
using CeMaS.Data.Streams;
using SenseLab.Common.Locations;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class StreamableRecord :
        Record,
        IStreamable
    {
        protected StreamableRecord(
            uint id,
            Guid sourceId,
            ITimeInterval temporalLocation,
            ISpatialLocation spatialLocation = null)
            : base(id, sourceId, temporalLocation, spatialLocation)
        {
        }

        public async static Task<StreamableRecord> Create(
            uint id,
            Guid sourceId,
            ITimeInterval temporalLocation,
            StreamInfo streamInfo,
            ISpatialLocation spatialLocation = null)
        {
            var record = new StreamableRecord(id, sourceId, temporalLocation, spatialLocation);
            await record.SetStreamInfo(streamInfo, true);
            return record;
        }

        public IStreamManager StreamManager
        {
            get { return streamInfo.StreamManager; }
        }
        public Stream Stream { get; private set; }
        public Stream StreamWritable { get; private set; }

        public async Task SetStreamManager(IStreamManager value)
        {
            value.ValidateNonNull(nameof(value));
            if (SetProperty(() => StreamManager, v => streamInfo.StreamManagerId = v.Id, value))
                await TransferStream();
        }
        public override string ToString()
        {
            return streamInfo.ToString();
        }

        [DataMember]
        private StreamInfo StreamInfo
        {
            get { return streamInfo; }
            set
            {
                SetStreamInfo(value, false).Wait();
            }
        }

        private async Task SetStreamInfo(StreamInfo value, bool createStreamWritable)
        {
            value.ValidateNonNull(nameof(StreamInfo));
            streamInfo = value;
            if (createStreamWritable)
                StreamWritable = await CreateStreamWritable();
            await OpenAndSetStream();
        }
        private async Task OpenAndSetStream()
        {
            Stream = await StreamManager.Open(streamInfo.NamespaceName, streamInfo.Name, true);
        }
        private async Task<Stream> CreateStreamWritable()
        {
            return await StreamManager.Create(streamInfo.NamespaceName, streamInfo.Name, true);
        }
        private async Task TransferStream()
        {
            var oldStream = Stream;
            oldStream.Flush();
            var newStream = await CreateStreamWritable();
            using (oldStream)
            using (newStream)
                oldStream.CopyTo(newStream);
            await OpenAndSetStream();
        }

        private StreamInfo streamInfo;
    }
}
