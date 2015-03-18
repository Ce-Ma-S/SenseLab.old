using CeMaS.Data.Streams;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Provides streamable records.
    /// </summary>
    public interface IStreamingRecordProvider :
        IRecordProvider,
        IUseStreamManager
    {
    }
}
