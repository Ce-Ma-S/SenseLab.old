using System.IO;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    public abstract class Serializer<T> :
        ISerializer<T>
    {
        public async Task Serialize(T item, Stream stream)
        {
            stream.ValidateNonNull("stream");
            await DoSerialize(item, stream);
        }
        public async Task<T> Deserialize(Stream stream)
        {
            stream.ValidateNonNull("stream");
            return await DoDeserialize(stream);
        }

        protected abstract Task DoSerialize(T item, Stream stream);
        protected abstract Task<T> DoDeserialize(Stream stream);
    }
}
