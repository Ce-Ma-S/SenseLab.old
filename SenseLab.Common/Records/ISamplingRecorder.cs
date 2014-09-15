using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Allows recording of changes or current states with extra sampling control.
    /// </summary>
    public interface ISamplingRecorder :
        IRecorder
    {
        /// <summary>
        /// Whether <see cref="RecordPeriod"/> is used.
        /// </summary>
        /// <value><see cref="TimeSpan.Zero"/> means no sampling.</value>
        TimeSpan RecordPeriodEnabled { get; set; }
        /// <summary>
        /// Sampling period between added sample records.
        /// </summary>
        /// <value><see cref="TimeSpan.Zero"/> means no sampling.</value>
        TimeSpan RecordPeriod { get; set; }

        /// <summary>
        /// Adds current sample record.
        /// Eg. magnetometer measurement.
        /// </summary>
        /// <returns>Added record.</returns>
        IRecord AddRecord();
    }
}
