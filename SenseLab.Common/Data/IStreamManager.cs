using System;
using System.IO;
using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    public interface IStreamManager
        : IId<Guid>
    {
        Task<Stream> OpenStreamForReading(string namespaceName, string name);
        Task<Stream> CreateStreamForWriting(string namespaceName, string name);
        Task<bool> RemoveStream(string namespaceName, string name);
    }
}
