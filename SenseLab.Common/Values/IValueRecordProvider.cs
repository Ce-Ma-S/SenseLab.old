using SenseLab.Common.Records;
using System;

namespace SenseLab.Common.Values
{
    public interface IValueRecordProvider :
        ISamplingRecordProvider
    {
        int BatchSize { get; set; }
        TimeSpan? BatchPeriod { get; set; }
    }
}
