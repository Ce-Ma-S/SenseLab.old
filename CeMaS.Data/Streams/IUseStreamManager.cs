using System.Threading.Tasks;

namespace CeMaS.Data.Streams
{
    public interface IUseStreamManager
    {
        /// <summary>
        /// Stream manager which has to be provided for proper functionality.
        /// </summary>
        IStreamManager StreamManager { get; }
        Task SetStreamManager(IStreamManager value);
    }
}
