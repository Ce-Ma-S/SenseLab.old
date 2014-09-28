using System;

namespace SenseLab.Common.Locations
{
    public static class TimeHelper
    {
        public static bool Contains(this ITime time, DateTimeOffset point)
        {
            return time != null && (time.From <= point && time.To >= point);
        }
        public static bool Intersects(this ITime time1, ITime time2)
        {
            return
                time1 != null && time2 != null &&
                (time1.Contains(time2.From) ||
                time1.Contains(time2.To));
        }
    }
}
