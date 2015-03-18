using System.IO;

namespace CeMaS.Data.Streams
{
    public interface IStreamable :
        IUseStreamManager
    {
        /// <summary>
        /// Read only stream.
        /// </summary>
        Stream Stream { get; }
    }
}