using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Values
{
    public interface IValuesRecord :
        IRecord
    {
        IEnumerable<ILocatableValue> Values { get; }
    }


    public interface IValuesRecord<T> :
        IValuesRecord
    {
        new IEnumerable<ILocatableValue<T>> Values { get; }
    }
}
