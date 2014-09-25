using System;

namespace SenseLab.Common.Locations
{
    public interface ITemporalLocation :
        ILocation
    {
        DateTimeOffset From { get; }
        TimeSpan Length { get; }
        DateTimeOffset To { get; }

        new ITemporalLocation Clone();
    }
}
