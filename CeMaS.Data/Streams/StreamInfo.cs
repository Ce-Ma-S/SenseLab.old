using CeMaS.Common.Validation;
using System;
using System.Runtime.Serialization;

namespace CeMaS.Data.Streams
{
    [DataContract]
    public class StreamInfo
    {
        public StreamInfo(Guid streamManagerId, string namespaceName, string name)
        {
            namespaceName.ValidateNonNullOrEmpty(nameof(namespaceName));
            name.ValidateNonNullOrEmpty(nameof(name));
            StreamManagerId = streamManagerId;
            NamespaceName = namespaceName;
            Name = name;
        }

        public IStreamManager StreamManager
        {
            get { return StreamManagers.Instance.GetItem(StreamManagerId); }
        }
        [DataMember]
        public Guid StreamManagerId { get; set; }
        [DataMember]
        public string NamespaceName { get; private set; }
        [DataMember]
        public string Name { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", NamespaceName, Name);
        }
    }
}
