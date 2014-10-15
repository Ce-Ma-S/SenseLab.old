using SenseLab.Common.Locations;
using System;
using System.Reactive.Linq;

namespace SenseLab.Common.Records
{
    public static class RecordsHelper
    {
        public static IQbservable<ITime> Times(this IQbservable<IRecord> records)
        {
            return records.Select(record => record.TemporalLocation).Distinct();
        }
        public static IQbservable<IRecord> DuringTime(this IQbservable<IRecord> records, ITime time)
        {
            return records.Where(record => record.TemporalLocation.Intersects(time));
        }

        public static IQbservable<Guid> SourceIds(this IQbservable<IRecord> records)
        {
            return records.Select(record => record.SourceId).Distinct();
        }
        public static IQbservable<IRecord> OfSource(this IQbservable<IRecord> records, Guid sourceId)
        {
            return records.Where(record => record.SourceId == sourceId);
        }

        public static IQbservable<Guid> GroupIds(this IQbservable<IRecord> records)
        {
            return records.Select(record => record.GroupId).Distinct();
        }
        public static IQbservable<IRecord> OfGroup(this IQbservable<IRecord> records, Guid groupId)
        {
            return records.Where(record => record.GroupId == groupId);
        }
    }
}
