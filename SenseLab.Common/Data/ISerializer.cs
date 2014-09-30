using System.IO;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    public interface ISerializer<T>
    {
        Task Serialize(T item, Stream stream);
        Task<T> Deserialize(Stream stream);
    }
}
