using SenseLab.Common.Data;
using System;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Allows recording of data streams.
    /// </summary>
    public interface IStreamingRecorder :
        IRecorder,
        IUseStreamManager
    {
    }
}
