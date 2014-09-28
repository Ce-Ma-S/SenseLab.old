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
        public static IQbservable<IRecordSource> Sources(this IQbservable<IRecord> records)
        {
            return records.Select(record => record.Source).Distinct();
        }
        public static IQbservable<IRecord> OfSource(this IQbservable<IRecord> records, Guid sourceId)
        {
            return records.Where(record => record.Source.Id == sourceId);
        }
        public static IQbservable<IRecordGroup> Groups(this IQbservable<IRecord> records)
        {
            return records.Select(record => record.Group).Distinct();
        }
        public static IQbservable<IRecord> OfGroup(this IQbservable<IRecord> records, Guid groupId)
        {
            return records.Where(record => record.Group != null && record.Group.Id == groupId);
        }
        public static IQbservable<IRecord> WithoutGroup(this IQbservable<IRecord> records)
        {
            return records.Where(record => record.Group == null);
        }
        public static IQbservable<IRecordType> Types(this IQbservable<IRecord> records)
        {
            return records.Select(record => record.Source.Type).Distinct();
        }
        public static IQbservable<IRecord> OfType(this IQbservable<IRecord> records, Guid typeId)
        {
            return records.Where(record => record.Source.Type.Id == typeId);
        }
    }
}
