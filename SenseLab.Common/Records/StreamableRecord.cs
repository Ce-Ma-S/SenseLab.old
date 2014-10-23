using SenseLab.Common.Data;
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
            Guid sourceId,
            uint sequenceNumber,
            ITime temporalLocation,
            IStreamManager streamManager,
            StreamInfo streamInfo,
            ISpatialLocation spatialLocation = null)
            : base(sourceId, sequenceNumber, temporalLocation, spatialLocation)
        {
            SetStreamManager(streamManager).Wait();
            SetStreamInfo(streamInfo);
        }

        public IStreamManager StreamManager
        {
            get { return streamManager; }
        }
        public Stream Stream { get; private set;}
        public Stream StreamWritable { get; private set;}

        public async static Task<StreamableRecord> Create(
            Guid sourceId,
            uint sequenceNumber,
            ITime temporalLocation,
            IStreamManager streamManager,
            StreamInfo streamInfo,
            ISpatialLocation spatialLocation = null)
        {
            var record = new StreamableRecord(sourceId, sequenceNumber, temporalLocation, streamManager, streamInfo, spatialLocation);
            await record.CreateEnd();
            return record;
        }

        public async Task SetStreamManager(IStreamManager value)
        {
            value.ValidateNonNull("StreamManager");
            var oldValue = streamManager;
            if (SetProperty(() => StreamManager, ref streamManager, value) && oldValue != null)
                await TransferStream(oldValue);
        }

        protected async Task CreateEnd()
        {
            StreamWritable = await CreateStreamWritable();
            await OpenAndSetStream();
        }
        protected override string GetText()
        {
            return string.Format("{0}: {1}", streamInfo.NamespaceName, streamInfo.Name);
        }

        [DataMember]
        private Guid StreamManagerId
        {
            get { return StreamManager.Id; }
            set
            {
                SetStreamManager(StreamManagers.Instance.GetFromId(value)).Wait();
            }
        }
        [DataMember]
        private StreamInfo StreamInfo
        {
            get { return streamInfo; }
            set
            {
                SetStreamInfo(value);
                OpenAndSetStream().Wait();
            }
        }

        private void SetStreamInfo(StreamInfo value)
        {
            value.ValidateNonNull("StreamInfo");
            streamInfo = value;
        }
        private async Task OpenAndSetStream()
        {
            Stream = await StreamManager.OpenStreamForReading(streamInfo.NamespaceName, streamInfo.Name);
        }
        private async Task<Stream> CreateStreamWritable()
        {
            return await StreamManager.CreateStreamForWriting(streamInfo.NamespaceName, streamInfo.Name);
        }
        private async Task TransferStream(IStreamManager oldManager)
        {
            var oldStream = Stream;
            oldStream.Flush();
            var newStream = await CreateStreamWritable();
            using (oldStream)
            using (newStream)
                oldStream.CopyTo(newStream);
            await OpenAndSetStream();
        }

        private IStreamManager streamManager;
        private StreamInfo streamInfo;
    }
}
