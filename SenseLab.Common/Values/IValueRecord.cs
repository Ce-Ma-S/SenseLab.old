using SenseLab.Common.Records;

namespace SenseLab.Common.Values
{
    public interface IValueRecord :
        IRecord
    {
        object Value { get; }
    }


    public interface IValueRecord<T> :
        IValueRecord
    {
        new T Value { get; }
    }
}
