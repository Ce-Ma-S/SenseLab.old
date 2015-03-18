using System;

namespace SenseLab.Common.Locations
{
    public interface ITimeInterval :
        ITemporalLocation
    {
        DateTimeOffset From { get; }
        TimeSpan Length { get; }
        DateTimeOffset To { get; }

        new ITimeInterval Clone();
    }
}
