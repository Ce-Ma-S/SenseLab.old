using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Allows extra sampling control of records.
    /// </summary>
    public interface ISamplingRecordProvider :
        IRecordProvider
    {
        /// <summary>
        /// Whether <see cref="RecordPeriod"/> is used.
        /// </summary>
        bool RecordPeriodEnabled { get; set; }
        /// <summary>
        /// Sampling period between added sample records.
        /// </summary>
        /// <value><see cref="TimeSpan.Zero"/> or negative period means no sampling.</value>
        TimeSpan RecordPeriod { get; set; }

        /// <summary>
        /// Adds current sample record.
        /// Eg. magnetometer measurement.
        /// </summary>
        /// <returns>Added record.</returns>
        Task<IRecord> AddRecord();
    }
}
