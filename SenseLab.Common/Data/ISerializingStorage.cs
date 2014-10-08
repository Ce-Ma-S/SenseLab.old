using System.Threading.Tasks;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Storage with items serialized to stream(s).
    /// </summary>
    public interface ISerializingStorage
    {
        #region Connection

        bool IsConnected { get; }

        Task Connect();
        Task Disconnect();

        #endregion

        #region Serialization

        Task SerializeItem(object item);
        Task<object> DeserializeItem(object itemId);

        #endregion
    }
}
