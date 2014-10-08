using System.IO;

namespace SenseLab.Common.Data
{
    public interface IStreamable :
        IUseStreamManager
    {
        Stream Stream { get; }
    }
}
