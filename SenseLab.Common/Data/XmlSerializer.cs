using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    public class XmlSerializer<T> :
        Serializer<T>
    {
        public static readonly XmlSerializer<T> Default = new XmlSerializer<T>(new DataContractJsonSerializer(typeof(T)));

        public XmlSerializer(XmlObjectSerializer serializer)
        {
            serializer.ValidateNonNull("serializer");
            Serializer = serializer;
        }
        public XmlObjectSerializer Serializer { get; private set; }

        protected override async Task DoSerialize(T item, Stream stream)
        {
            await Task.Run(() => Serializer.WriteObject(stream, item));
        }
        protected override async Task<T> DoDeserialize(Stream stream)
        {
            return await Task.Run(() => (T)Serializer.ReadObject(stream));
        }
    }
}
