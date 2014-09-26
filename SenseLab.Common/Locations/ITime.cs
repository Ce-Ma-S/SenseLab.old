using System;

namespace SenseLab.Common.Locations
{
    public interface ITime :
        ITemporalLocation
    {
        DateTimeOffset From { get; }
        TimeSpan Length { get; }
        DateTimeOffset To { get; }
    }
}
