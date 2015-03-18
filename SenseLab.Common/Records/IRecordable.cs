using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Recordable object.
    /// </summary>
    public interface IRecordable :
        IRecordSource
    {
        /// <summary>
        /// Creates record provider.
        /// </summary>
        Task<IRecordProvider> CreateRecordProvider();
    }
}
