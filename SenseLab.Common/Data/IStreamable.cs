using System.IO;

namespace SenseLab.Common.Data
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
