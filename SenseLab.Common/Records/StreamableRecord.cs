using SenseLab.Common.Data;
using SenseLab.Common.Locations;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace SenseLab.Common.Records
{
    [DataContract]
    public class StreamableRecord :
        Record,
        IStreamable
    {
        public StreamableRecord(
            IRecordSource source,
            uint sequenceNumber,
            ITime temporalLocation,
            IStreamManager streamManager,
            StreamInfo streamInfo,
            ISpatialLocation spatialLocation = null)
            : base(source, sequenceNumber, temporalLocation, spatialLocation)
        {
        }

        public IStreamManager StreamManager
        {
            get { return streamManager; }
            set
            {
                value.ValidateNonNull("streamManager");
                var oldValue = streamManager;
                if (SetProperty(() => StreamManager, ref streamManager, value) && oldValue != null)
                    TransferStream(oldValue, value);
            }
        }
        public Stream Stream
        {
            get { return stream; }
            private set
            {
                SetProperty(() => Stream, ref stream, value);
            }
        }

        protected override string GetText()
        {
            return string.Format("{0}: {1}", streamInfo.NamespaceName, streamInfo.Name);
        }

        [DataMember]
        private Guid StreamManagerId
        {
            get { return StreamManager.Id; }
            set { StreamManager = StreamManagers.Instance.GetFromId(value); }
        }
        [DataMember]
        private StreamInfo StreamInfo
        {
            get { return streamInfo; }
            set
            {
                value.ValidateNonNull("StreamInfo");
                streamInfo = value;
                SetStream();
            }
        }

        private void SetStream()
        {
            StreamManager.OpenStreamForReading(streamInfo.NamespaceName, streamInfo.Name)
                .ContinueWith(t => Stream = t.Result);
        }
        private void TransferStream(IStreamManager oldManager, IStreamManager newManager)
        {
            newManager.CreateStreamForWriting(streamInfo.NamespaceName, streamInfo.Name)
                .ContinueWith(
                    t =>
                    {
                        var oldStream = Stream;
                        using (oldStream)
                        using (var newStream = t.Result)
                        {
                            oldStream.CopyTo(newStream);
                        }
                        SetStream();
                    });
        }

        private IStreamManager streamManager;
        private StreamInfo streamInfo;
        private Stream stream;
    }
}
