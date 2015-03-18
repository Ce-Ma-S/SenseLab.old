using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Locations
{
    public static class TimeHelper
    {
        public static ITimeInterval Including(this ITimeInterval value1, ITimeInterval value2)
        {
            value1.ValidateNonNull(nameof(value2));
            return new TimeInterval(
                value1.From <= value2.From ?
                    value1.From :
                    value2.From,
                value1.To >= value2.To ?
                    value1.To :
                    value2.To);
        }
        public static bool Contains(this ITimeInterval time, DateTimeOffset point)
        {
            return time != null && (time.From <= point && time.To >= point);
        }
        public static bool Intersects(this ITimeInterval time1, ITimeInterval time2)
        {
            return
                time1 != null && time2 != null &&
                (time1.Contains(time2.From) ||
                time1.Contains(time2.To));
        }
    }
}
