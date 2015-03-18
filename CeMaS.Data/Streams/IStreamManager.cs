using CeMaS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CeMaS.Data.Streams
{
    /// <summary>
    /// Provides streams.
    /// </summary>
    public interface IStreamManager :
        IId<Guid>,
        IDisposable
    {
        /// <summary>
        /// Whether this provider is only for reading.
        /// </summary>
        bool IsReadOnly { get; }
        IEnumerable<string> Namespaces { get; }
        IEnumerable<string> GetNames(string namespaceName);

        Task<Stream> Contains(string namespaceName, string name);
        Task<Stream> Open(string namespaceName, string name, bool readOnly);
        Task<Stream> Create(string namespaceName, string name, bool rewriteExisting);
        Task<bool> Remove(string namespaceName, string name);
    }
}
